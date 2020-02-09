using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Items;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.SystemServices;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Services
{
    public partial class ServicesUserControl : UserControl, IUserControl
    {
        private ItemsMonitoringService _itemsMonitoringService;
        private Dictionary<Guid, ServiceUC> _indexes;
        public Guid _agentId;

        public ServicesUserControl(Guid agentId, ItemsMonitoringService itemsMonitoringService)
        {
            InitializeComponent();
            _agentId = agentId;
            _itemsMonitoringService = itemsMonitoringService;
            _indexes = new Dictionary<Guid, ServiceUC>();
            Populate();
        }

        public void PopulateByDictionary()
        {
            this.wrapPanel.Children.Clear();

            foreach (var itemViewModel in _indexes)
            {
                this.wrapPanel.Children.Add(_indexes[itemViewModel.Key]);
            }
            OnApplyTemplate();
        }

        ~ServicesUserControl()
        {
            _indexes.Clear();
            _indexes = null;
            _itemsMonitoringService = null;
        }

        public void Populate()
        {
            _indexes.Clear();
            this.wrapPanel.Children.Clear();
            _itemsMonitoringService.GetSystemServices(_agentId).ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                {
                    foreach (SystemServiceViewModel serviceViewModel in task.Result.Success.Items)
                    {
                        _indexes.Add(serviceViewModel.Id, new ServiceUC(OnRemove, OnEdit, serviceViewModel, _itemsMonitoringService));
                    }
                    PopulateByDictionary();
                }
                else
                    MessageBox.Show("Falha na requisição de agents", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OnEdit(object sender, EventArgs e)
        {
            Guid agentId = (Guid)sender;

        }

        private void OnRemove(object sender, EventArgs e)
        {
            SystemServiceViewModel serviceViewModel = sender as SystemServiceViewModel;

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

        private async void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FrmItemsAdd frmItemsAdd = new FrmItemsAdd(new ServicesCreateUC(_agentId));

            var dialogResult = frmItemsAdd.ShowDialog();

            if (dialogResult.HasValue && dialogResult.Value)
            {
                await _itemsMonitoringService.Post(frmItemsAdd.Item);
            }
        }

        private void btnRefrash_Click(object sender, RoutedEventArgs e)
        {
            Populate();
        }
    }
}
