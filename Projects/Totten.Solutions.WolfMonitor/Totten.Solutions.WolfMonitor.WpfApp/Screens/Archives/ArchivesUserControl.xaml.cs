using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Agents;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Items;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Archives;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Items;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Archives
{
    /// <summary>
    /// Interação lógica para ArchivesUserControl.xam
    /// </summary>
    public partial class ArchivesUserControl : UserControl
    {
        private TaskScheduler _currentTaskScheduler;
        private AgentService _agentService;
        private UserBasicInformationViewModel _userBasicInformation;
        private ItemsMonitoringService _itemsMonitoringService;
        private Dictionary<Guid, ArchiveUC> _indexes;
        private Guid _agentId;

        public ArchivesUserControl(Guid agentId,
                                   UserBasicInformationViewModel userBasicInformation,
                                   ItemsMonitoringService itemsMonitoringService,
                                   AgentService agentService)
        {
            InitializeComponent();
            _agentId = agentId;
            _userBasicInformation = userBasicInformation;
            _agentService = agentService;
            _itemsMonitoringService = itemsMonitoringService;
            _indexes = new Dictionary<Guid, ArchiveUC>();
            _currentTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            if (_userBasicInformation.UserLevel < (int)EUserLevel.Admin)
            {
                btnAdd.IsEnabled = false;
                btnAdd.Visibility = Visibility.Collapsed;
            }

            Populate();
        }

        ~ArchivesUserControl()
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

        public void Populate()
        {
            _indexes.Clear();
            this.wrapPanel.Children.Clear();

            _itemsMonitoringService.GetArchives(_agentId).ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                {
                    foreach (ArchiveViewModel viewModel in task.Result.Success.Items)
                        _indexes.Add(viewModel.Id, new ArchiveUC(OnRemove, OnEdit, viewModel, _userBasicInformation));

                    PopulateByDictionary();
                }
                else
                    MessageBox.Show("Falha na busca dos arquivos do agent", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async void OnModified(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente modificar o arquivo?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var view = (ArchiveViewModel)sender;

                var returned = await _agentService.PostSolicitation(new ItemSolicitationVO
                {
                    ItemId = view.Id,
                    AgentId = _agentId,
                    Name = view.Name,
                    SolicitationType = SolicitationType.ChangeFile,
                    DisplayName = view.DisplayName,
                    NewValue = view.Value
                });
                if (returned.IsSuccess)
                {
                    MessageBox.Show("Foi enviada uma solicitação para o agent.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                    Populate();
                }
                else
                    MessageBox.Show($"Falha na solicitação.\nMensagem: {returned.Failure.Message}", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
           
            }
        }

        private void OnEdit(object sender, EventArgs e)
        {
            var detail = new ArchiveDetailWindow(OnModified, (ArchiveViewModel)sender, _itemsMonitoringService, _userBasicInformation);
            detail.ShowDialog();
        }

        private void OnRemove(object sender, EventArgs e)
        {
            ArchiveViewModel serviceViewModel = sender as ArchiveViewModel;

            if (MessageBox.Show($"Deseja realmente remover o arquivo: {serviceViewModel.DisplayName} do monitoramento?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
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
            FrmItemsAdd frmItemsAdd = new FrmItemsAdd(new ArchivesCreateUC(_agentId));

            var dialogResult = frmItemsAdd.ShowDialog();

            if (dialogResult.HasValue && dialogResult.Value)
            {
                await _itemsMonitoringService.PostArchive(frmItemsAdd.Item);
                Populate();
            }
        }

        private void btnRefrash_Click(object sender, RoutedEventArgs e)
           => Populate();

        public void SetDataOnGrid(List<KeyValuePair<Guid, ArchiveUC>> list)
        {
            this.wrapPanel.Children.Clear();

            foreach (var itemViewModel in list)
                this.wrapPanel.Children.Add(_indexes[itemViewModel.Key]);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        => SetDataOnGrid(_indexes.Where(x => x.Value.lblDisplayName.Text.Contains(txtArchiveName.Text, StringComparison.OrdinalIgnoreCase)).ToList());


        private void txtArchiveName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtArchiveName.Text))
                SetDataOnGrid(_indexes.ToList());
        }
    }
}
