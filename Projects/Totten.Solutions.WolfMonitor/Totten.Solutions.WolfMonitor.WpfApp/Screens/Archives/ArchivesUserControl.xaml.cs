﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Agents;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Items;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Archives;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Archives
{
    /// <summary>
    /// Interação lógica para ArchivesUserControl.xam
    /// </summary>
    public partial class ArchivesUserControl : UserControl
    {
        private TaskScheduler currentTaskScheduler;
        private AgentService _agentService;
        private ItemsMonitoringService _itemsMonitoringService;
        private Dictionary<Guid, ArchiveUC> _indexes;
        private Guid _agentId;

        public ArchivesUserControl(Guid agentId,
                                   ItemsMonitoringService itemsMonitoringService,
                                   AgentService agentService)
        {
            InitializeComponent();
            _agentId = agentId;
            _agentService = agentService;
            _itemsMonitoringService = itemsMonitoringService;
            _indexes = new Dictionary<Guid, ArchiveUC>();
            currentTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

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
                    foreach (ArchiveViewModel serviceViewModel in task.Result.Success.Items)
                        _indexes.Add(serviceViewModel.Id, new ArchiveUC(OnRemove, OnEdit, serviceViewModel));

                    PopulateByDictionary();
                }
                else
                    MessageBox.Show("Falha na busca dos arquivos do agent", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
            
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OnEdit(object sender, EventArgs e)
        {
            var detail = new ArchiveDetailWindow((ArchiveViewModel)sender, _itemsMonitoringService);
            detail.ShowDialog();
        }

        private void OnRemove(object sender, EventArgs e)
        {
            ArchiveViewModel serviceViewModel = sender as ArchiveViewModel;

            if (MessageBox.Show($"Deseja realmente remover o serviço: {serviceViewModel.DisplayName} do monitoramento?", "Atênção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
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
            }
        }

        private void btnRefrash_Click(object sender, RoutedEventArgs e)
           => Populate();
    }
}
