using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Items;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.SystemServices;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Services
{
    public partial class ServiceDetailWindow : Window
    {
        private int take = 10;
        private int skip = 0;
        private int _actualPage = 1;
        private int _qtItems;

        private ItemsMonitoringService _itemsMonitoringService;
        private SystemServiceViewModel _systemServiceView;

        public ServiceDetailWindow(SystemServiceViewModel systemServiceView,
                                   ItemsMonitoringService itemsMonitoringService)
        {
            InitializeComponent();
            _systemServiceView = systemServiceView;
            _itemsMonitoringService = itemsMonitoringService;
            Populate();
        }
        private void GetHistoricItems()
        {
            _itemsMonitoringService.GetItemHistoric(_systemServiceView.Id, $"{take}", $"{skip}")
             .ContinueWith(task =>
             {
                 Result<Exception, PageResult<ItemHistoricViewModel>> result = task.Result;

                 if (result.IsSuccess)
                 {
                     if (result.Success.Items.Count > 0)
                     {
                         _qtItems = int.Parse(result.Success.Count);

                         gridHistoric.DataContext = result.Success.Items;

                         if (result.Success.Items.Count < take || skip > _qtItems)
                             btnNextPage.IsEnabled = false;

                     }
                 }
             }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        private void Populate(int page = 1)
        {
            this.lblDisplayName.Text = _systemServiceView.DisplayName;
            this.lblName.Text = _systemServiceView.Name;
            this.lblCurrentValue.Text = _systemServiceView.Value;
            this.lblMonitoredAt.Text = _systemServiceView.MonitoredAt;
            GetHistoricItems();
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            skip -= take;
            btnActualPage.Content = $"{--_actualPage}";

            if (skip < _qtItems)
                btnNextPage.IsEnabled = true;

            if (tabControl.SelectedIndex == 0)
            {
                GetHistoricItems();
                if (_actualPage == 1)
                    btnPrevPage.IsEnabled = false;
            }
            else
            {

            }

        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
            {
                btnActualPage.Content = $"{++_actualPage}";
                skip += take;
                GetHistoricItems();
            }
            else
            {

            }
            btnPrevPage.IsEnabled = true;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = 1;
        }
    }
}
