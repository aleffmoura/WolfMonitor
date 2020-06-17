using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Agents;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Agents;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Agents
{
    /// <summary>
    /// Interação lógica para AgentsUserControl.xam
    /// </summary>
    public partial class AgentsUserControl : UserControl, IUserControl
    {
        private AgentService _agentService;
        private ItemsMonitoringService _itensMonitoringService;
        private Dictionary<Guid, AgentUC> _indexes;

        private UserBasicInformationViewModel _userBasicInformation;
        private EventHandler _onSwitchControl;

        public AgentsUserControl(AgentService agentService,
                                 ItemsMonitoringService itensMonitoringService,
                                 EventHandler onSwitchControl,
                                 UserBasicInformationViewModel userBasicInformation)
        {
            InitializeComponent();
            _agentService = agentService;
            _onSwitchControl = onSwitchControl;
            _itensMonitoringService = itensMonitoringService;
            _userBasicInformation = userBasicInformation;
            _indexes = new Dictionary<Guid, AgentUC>();

            if (_userBasicInformation.UserLevel < (int)EUserLevel.Admin)
            {
                btnAdd.IsEnabled = false;
                btnAdd.Visibility = Visibility.Collapsed;
            }
        }

        ~AgentsUserControl()
        {
            _indexes.Clear();
            _indexes = null;
            _agentService = null;
        }

        public void PopulateByDictionary()
        {
            this.wrapPanel.Children.Clear();

            foreach (var agentViewModel in _indexes)
                this.wrapPanel.Children.Add(_indexes[agentViewModel.Key]);
            
            OnApplyTemplate();
        }

        public void Populate()
        {
            _indexes.Clear();
            this.wrapPanel.Children.Clear();
            var loading = new LoadingWindow(_agentService.GetAllAgentsByCompany().ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                {
                    foreach (AgentResumeViewModel agentViewModel in task.Result.Success.Items)
                    {
                        _indexes.Add(agentViewModel.Id, new AgentUC(OnRemove, OnEdit, agentViewModel, _userBasicInformation));
                    }
                    PopulateByDictionary();
                }
                else
                    MessageBox.Show("Falha na requisição de agents", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
            }, TaskScheduler.FromCurrentSynchronizationContext()));

            loading.ShowDialog();
        }

        private void OnEdit(object sender, EventArgs e)
            => _onSwitchControl?.Invoke(new AgentDetailUC((Guid)sender,_userBasicInformation, _agentService, _itensMonitoringService), new EventArgs());

        private void OnRemove(object sender, EventArgs e)
        {
            AgentResumeViewModel agentViewModel = sender as AgentResumeViewModel;

            if (MessageBox.Show($"Deseja realmente remover o serviço: {agentViewModel.DisplayName} do monitoramento?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _agentService.Delete(agentViewModel.Id).ContinueWith(task =>
                {
                    if (task.Result.IsSuccess)
                    {
                        MessageBox.Show($"Removido com sucesso", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                        _indexes.Remove(agentViewModel.Id);
                        PopulateByDictionary();
                    }
                    else
                        MessageBox.Show($"Falha na tentativa de remoção.", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AgentCreateWindow agentCreateWindow = new AgentCreateWindow(_agentService);
            var result = agentCreateWindow.ShowDialog();

            if (result.HasValue && result.Value)
                Populate();
        }

        public void SetDataOnGrid(List<KeyValuePair<Guid, AgentUC>> list)
        {
            this.wrapPanel.Children.Clear();

            foreach (var itemViewModel in list)
                this.wrapPanel.Children.Add(_indexes[itemViewModel.Key]);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        => SetDataOnGrid(_indexes.Where(x => x.Value.lblDisplayName.Text.Contains(txtAgentName.Text, StringComparison.OrdinalIgnoreCase)).ToList());

        private void txtAgentName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtAgentName.Text))
                SetDataOnGrid(_indexes.ToList());
        }
    }
}
