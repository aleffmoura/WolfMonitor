using System;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Archives;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Items;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Archives
{
    /// <summary>
    /// Lógica interna para ServiceDetailWindow.xaml
    /// </summary>
    public partial class ArchiveDetailWindow : Window
    {
        private int _take = 10;
        private int _skip = 0;
        private int _actualPage = 1;
        private int _qtItems;

        private ItemsMonitoringService _itemsMonitoringService;
        private ArchiveViewModel _archiveViewModel;

        public ArchiveDetailWindow(ArchiveViewModel archiveViewModel, ItemsMonitoringService itemsMonitoringService)
        {
            _archiveViewModel = archiveViewModel;
            _itemsMonitoringService = itemsMonitoringService;
            InitializeComponent();
            Populate();
        }

        private void Populate()
        {
            this.lblDisplayName.Text = _archiveViewModel.DisplayName;
            this.lblMonitoredAt.Text = _archiveViewModel.MonitoredAt;
            GetHistoricItems();
        }

        private Task GetHistoricItems()
        {
            return _itemsMonitoringService.GetItemHistoric(_archiveViewModel.Id, $"{_take}", $"{_skip}")
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
            return _itemsMonitoringService.GetSolicitationsHistoric(_archiveViewModel.Id, $"{_take}", $"{_skip}")
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

    }
}
