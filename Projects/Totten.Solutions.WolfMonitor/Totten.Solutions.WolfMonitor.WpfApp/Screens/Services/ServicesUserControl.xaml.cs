using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Agents;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Items;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Items;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.SystemServices;
using Timer = System.Timers.Timer;
namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Services
{
    public partial class ServicesUserControl : UserControl, IUserControl
    {
        private TaskScheduler currentTaskScheduler;
        private AgentService _agentService;
        private ItemsMonitoringService _itemsMonitoringService;
        private UserBasicInformationViewModel _userBasicInformation;
        private Dictionary<Guid, ServiceUC> _indexes;
        private Guid _agentId;
        private Timer _autoRefresh;

        public ServicesUserControl(Guid agentId,
                                   UserBasicInformationViewModel userBasicInformation,
                                   ItemsMonitoringService itemsMonitoringService,
                                   AgentService agentService)
        {
            InitializeComponent();
            _agentId = agentId;
            _userBasicInformation = userBasicInformation;
            _agentService = agentService;
            _itemsMonitoringService = itemsMonitoringService;
            _indexes = new Dictionary<Guid, ServiceUC>();
            currentTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            if (_userBasicInformation.UserLevel < (int)EUserLevel.Admin)
            {
                btnAdd.IsEnabled = false;
                btnAdd.Visibility = Visibility.Collapsed;
            }

            Populate();
            LoadCombobox();
        }

        ~ServicesUserControl()
        {
            _indexes.Clear();
            _indexes = null;
            _itemsMonitoringService = null;
        }

        public void PopulateByDictionary()
        {
            this.wrapPanel.Children.Clear();

            foreach (var itemViewModel in _indexes)
                this.wrapPanel.Children.Add(_indexes[itemViewModel.Key]);

            OnApplyTemplate();
        }

        private void LoadCombobox() => cbTimeRefesh.ItemsSource = new List<string>
            {
                new string("segundos"),
                new string("minutos")
            };

        public void Populate()
        {
            _indexes.Clear();
            this.wrapPanel.Children.Clear();
            _itemsMonitoringService.GetSystemServices(_agentId).ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                {
                    foreach (SystemServiceViewModel serviceViewModel in task.Result.Success.Items)
                        _indexes.Add(serviceViewModel.Id, new ServiceUC(OnRemove, OnEdit, OnRestart, serviceViewModel, _userBasicInformation));

                    PopulateByDictionary();
                }
                else
                    MessageBox.Show("Falha na busca dos serviços do agent", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async void OnRestart(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente modificar o status do serviço?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var view = (SystemServiceViewModel)sender;

                string value = string.Empty;

                EStatusService statusEnum = (EStatusService)Enum.Parse(typeof(EStatusService), view.Value);

                if (statusEnum == EStatusService.Active || statusEnum == EStatusService.Failed)
                    value = EStatusService.Inactive.ToString();
                else if (statusEnum == EStatusService.Inactive)
                    value = EStatusService.Active.ToString();
                else if (statusEnum == EStatusService.Running)
                    value = EStatusService.Stopped.ToString();
                else if (statusEnum == EStatusService.Stopped)
                    value = EStatusService.Running.ToString();
                else
                    value = EStatusService.Running.ToString();

                var returned = await _agentService.PostSolicitation(new ItemSolicitationVO
                {
                    ItemId = view.Id,
                    AgentId = _agentId,
                    Name = view.Name,
                    SolicitationType = SolicitationType.ChangeStatus,
                    DisplayName = view.DisplayName,
                    NewValue = value
                });

                if (returned.IsSuccess)
                    MessageBox.Show("Foi enviada uma solicitação para o agent.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show($"Falha na solicitação.\nMensagem: {returned.Failure.Message}", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnEdit(object sender, EventArgs e)
        {
            ServiceDetailWindow serviceDetail = new ServiceDetailWindow((SystemServiceViewModel)sender, _itemsMonitoringService);
            serviceDetail.ShowDialog();
        }

        private void OnRemove(object sender, EventArgs e)
        {
            SystemServiceViewModel serviceViewModel = sender as SystemServiceViewModel;

            if (MessageBox.Show($"Deseja realmente remover o serviço: {serviceViewModel.DisplayName} do monitoramento?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _itemsMonitoringService.Delete(_agentId, serviceViewModel.Id).ContinueWith(task =>
                {
                    if (task.Result.IsSuccess)
                    {
                        MessageBox.Show($"Removido com sucesso", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                        _indexes.Remove(serviceViewModel.Id);
                        PopulateByDictionary();
                    }
                    else
                    {
                        MessageBox.Show($"Falha na tentativa de remoção.", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrmItemsAdd frmItemsAdd = new FrmItemsAdd(new ServicesCreateUC(_agentId));

            var dialogResult = frmItemsAdd.ShowDialog();

            if (dialogResult.HasValue && dialogResult.Value)
            {
                await _itemsMonitoringService.PostService(frmItemsAdd.Item);
                Populate();
            }
        }

        private void btnRefrash_Click(object sender, RoutedEventArgs e)
           => Populate();

        private void txtValueRefresh_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnApplyTimer_Click(object sender, RoutedEventArgs e)
        {
            if (_autoRefresh == null)
            {
                if (string.IsNullOrEmpty(txtValueRefresh.Text))
                {
                    MessageBox.Show($"Preencha o valor do tempo para o atualização automática", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (cbTimeRefesh.SelectedIndex < 0)
                {
                    MessageBox.Show($"Selecione como será o intervalo de tempo em segundos/minutos para atualização", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (int.TryParse(txtValueRefresh.Text, out int value))
                {
                    var mili = cbTimeRefesh.SelectedIndex == 0 ? value * 1000 : value * 60000;
                    _autoRefresh = new Timer(mili);
                    _autoRefresh.Enabled = true;
                    _autoRefresh.Elapsed += AutoRefresh;
                    kindTimer.Kind = MaterialDesignThemes.Wpf.PackIconKind.Stop;
                    txtValueRefresh.IsEnabled = false;
                    cbTimeRefesh.IsEnabled = false;
                    return;
                }
                MessageBox.Show($"Valor informado não é um numero inteiro válido", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                _autoRefresh.Enabled = false;
                _autoRefresh.Elapsed -= AutoRefresh;
                _autoRefresh.Dispose();
                _autoRefresh = null;
                txtValueRefresh.IsEnabled = true;
                cbTimeRefesh.IsEnabled = true;
                kindTimer.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
            }
        }

        private void AutoRefresh(object sender, ElapsedEventArgs e)
        {
            _itemsMonitoringService.GetSystemServices(_agentId).ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                {
                    foreach (SystemServiceViewModel serviceViewModel in task.Result.Success.Items)
                    {
                        if (_indexes.TryGetValue(serviceViewModel.Id, out ServiceUC value))
                        {
                            value.SetServiceValues(serviceViewModel);
                        }
                    }
                }
                else
                    MessageBox.Show("Falha na busca dos serviços do agent", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
            }, currentTaskScheduler);
        }


        public void SetDataOnGrid(List<KeyValuePair<Guid, ServiceUC>> list)
        {
            this.wrapPanel.Children.Clear();

            foreach (var itemViewModel in list)
                this.wrapPanel.Children.Add(_indexes[itemViewModel.Key]);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        => SetDataOnGrid(_indexes.Where(x => x.Value.lblServiceName.Text.Contains(txtServiceName.Text, StringComparison.OrdinalIgnoreCase) ||
                                                      x.Value.lblDisplayName.Text.Contains(txtServiceName.Text, StringComparison.OrdinalIgnoreCase)).ToList());

        private void txtServiceName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtServiceName.Text))
                SetDataOnGrid(_indexes.ToList());
        }
    }
}
