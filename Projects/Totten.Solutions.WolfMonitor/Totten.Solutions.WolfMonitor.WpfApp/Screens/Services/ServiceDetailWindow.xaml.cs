using System;
using System.Collections.Generic;
using System.Linq;
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
        private int _currentTab = 0;
        private int _take = 10;
        private int _skip = 0;
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
        
        private Task GetHistoricItems()
        {
            return _itemsMonitoringService.GetItemHistoric(_systemServiceView.Id, $"{_take}", $"{_skip}")
             .ContinueWith(task =>
             {
                 Result<Exception, PageResult<ItemHistoricViewModel>> result = task.Result;

                 if (result.IsSuccess)
                 {
                     if (result.Success.Items.Count > 0)
                     {
                         _qtItems = int.Parse(result.Success.Count);

                         gridHistoric.DataContext = result.Success.Items.OrderBy(x => x.MonitoredAt).ToList();
                         if (result.Success.Items.Count < _take || _skip > _qtItems)
                             btnNextPage.IsEnabled = false;
                         else
                             btnNextPage.IsEnabled = true;
                     }
                 }

                 return btnNextPage.IsEnabled;
             }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private Task GetSolicitations()
        {
            return _itemsMonitoringService.GetSolicitationsHistoric(_systemServiceView.Id, $"{_take}", $"{_skip}")
             .ContinueWith(task =>
             {
                 Result<Exception, PageResult<ItemSolicitationViewModel>> result = task.Result;

                 if (result.IsSuccess)
                 {
                     if (result.Success.Items.Count > 0)
                     {
                         _qtItems = int.Parse(result.Success.Count);

                         gridSolicitations.DataContext = result.Success.Items;

                         if (result.Success.Items.Count < _take || _skip > _qtItems)
                             btnNextPage.IsEnabled = false;
                         else
                             btnNextPage.IsEnabled = true;
                     }
                 }
             }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        
        private void Populate()
        {
            this.lblDisplayName.Text = _systemServiceView.DisplayName;
            this.lblName.Text = _systemServiceView.Name;
            this.lblCurrentValue.Text = _systemServiceView.Value;
            this.lblMonitoredAt.Text = _systemServiceView.MonitoredAt;
            GetHistoricItems();
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            _skip -= _take;
            btnActualPage.Content = $"{--_actualPage}";
            btnPrevPage.IsEnabled = false;
            btnNextPage.IsEnabled = false;

            Task task;

            if (tabControl.SelectedIndex == 0)
                task = GetHistoricItems();
            else
                task = GetSolicitations();

            task.ContinueWith(task =>
            {
                btnPrevPage.IsEnabled = true;
                if (_actualPage == 1)
                    btnPrevPage.IsEnabled = false;
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            btnActualPage.Content = $"{++_actualPage}";
            _skip += _take;
            btnNextPage.IsEnabled = false;
            btnPrevPage.IsEnabled = false;

            Task task;

            if (tabControl.SelectedIndex == 0)
                task = GetHistoricItems();
            else
                task = GetSolicitations();

            task.ContinueWith(task =>
            {
                if (_actualPage != 1)
                    btnPrevPage.IsEnabled = true;

            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_currentTab == tabControl.SelectedIndex)
                return;

            _skip = 0;
            _actualPage = 1;
            _qtItems = 0;
            btnActualPage.Content = $"{_actualPage}";
            btnPrevPage.IsEnabled = false;
            btnNextPage.IsEnabled = false;

            _currentTab = tabControl.SelectedIndex;

            if (tabControl.SelectedIndex == 0)
            {
                GetHistoricItems();
                return;
            }

            GetSolicitations();
        }
    }
}
