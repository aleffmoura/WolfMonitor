﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Agents;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Items;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Agents;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Agents
{
    /// <summary>
    /// Interação lógica para AgentsUserControl.xam
    /// </summary>
    public partial class AgentsUserControl : UserControl, IUserControl
    {
        private AgentService _agentService;
        private ItensMonitoringService _itensMonitoringService;
        private Dictionary<Guid, AgentUC> _indexes;

        private EventHandler _onSwitchControl;

        public AgentsUserControl(AgentService agentService,
                                ItensMonitoringService itensMonitoringService,
                                EventHandler onSwitchControl)
        {
            InitializeComponent();
            _agentService = agentService;
            _onSwitchControl = onSwitchControl;
            _itensMonitoringService = itensMonitoringService;
            _indexes = new Dictionary<Guid, AgentUC>();
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
            {
                this.wrapPanel.Children.Add(_indexes[agentViewModel.Key]);
            }
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
                        _indexes.Add(agentViewModel.Id, new AgentUC(OnRemove, OnEdit, agentViewModel));
                    }
                    PopulateByDictionary();
                }
                else
                    MessageBox.Show("Falha na requisição de agents", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
            }, TaskScheduler.FromCurrentSynchronizationContext()));
            
            loading.ShowDialog();

        }
        private void OnEdit(object sender, EventArgs e)
        {
            Guid agentId = (Guid)sender;

            _onSwitchControl?.Invoke(new AgentDetailUC(agentId, _agentService, _itensMonitoringService), new EventArgs());

        }
        private void OnRemove(object sender, EventArgs e)
        {
            AgentResumeViewModel agentViewModel = sender as AgentResumeViewModel;

            if (MessageBox.Show($"Deseja realmente remover o serviço: {agentViewModel.DisplayName} do monitoramento?", "Atênção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
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
                    {
                        MessageBox.Show($"Falha na tentativa de remoção.", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AgentCreateWindow agentCreateWindow = new AgentCreateWindow(_agentService);
            agentCreateWindow.ShowDialog();
        }
    }
}