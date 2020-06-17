using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
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
        private int _currentTab = 0;
        private int _take = 10;
        private int _skip = 0;
        private int _actualPage = 1;
        private int _qtItems;
        private EventHandler _onModified;

        private ItemsMonitoringService _itemsMonitoringService;
        private ArchiveViewModel _archiveViewModel;
        private UserBasicInformationViewModel _userBasicInformation;

        public ArchiveDetailWindow(EventHandler modified,
                                   ArchiveViewModel archiveViewModel,
                                   ItemsMonitoringService itemsMonitoringService,
                                   UserBasicInformationViewModel userBasicInformation)
        {
            _onModified = modified;
            _archiveViewModel = archiveViewModel;
            _itemsMonitoringService = itemsMonitoringService;
            _userBasicInformation = userBasicInformation;
            InitializeComponent();
            Populate();


            if (_userBasicInformation.UserLevel < (int)EUserLevel.Admin)
            {
                btnEdit.IsEnabled = false;
                btnEdit.Visibility = Visibility.Collapsed;
            }
        }

        private void btnModified_Click(object sender, RoutedEventArgs e)
        {
            if (kindEdit.Kind != MaterialDesignThemes.Wpf.PackIconKind.ContentSaveEdit)
            {
                txtNotePad.IsReadOnly = false;
                kindEdit.Kind = MaterialDesignThemes.Wpf.PackIconKind.ContentSaveEdit;
                btnCancel.Visibility = Visibility.Visible;
                return;
            }

            if (_archiveViewModel.Value.Equals(txtNotePad.Text))
            {
                MessageBox.Show("Não foi alterado nenhum valor pois o conteúdo não sofreu mudanças", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                btnCancel_Click(sender, e);
                return;
            }

            _archiveViewModel.Value = txtNotePad.Text;

            _onModified?.Invoke(_archiveViewModel, new EventArgs());
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            txtNotePad.Text = _archiveViewModel.Value;
            txtNotePad.IsReadOnly = true;
            kindEdit.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
            btnCancel.Visibility = Visibility.Collapsed;
        }

        private void Populate()
        {
            this.lblName.Text = _archiveViewModel.Name;
            this.lblDisplayName.Text = _archiveViewModel.DisplayName;
            this.lblMonitoredAt.Text = _archiveViewModel.MonitoredAt;
            this.txtNotePad.Text = _archiveViewModel.Value;
            GetHistoricItems();
            GetSolicitations();
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

        private void tabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
            {
                _currentTab = 0;
                pnlBottom.Visibility = Visibility.Collapsed;
                return;
            }

            if (_currentTab == tabControl.SelectedIndex)
                return;

            _skip = 0;
            _actualPage = 1;
            _qtItems = 0;
            btnActualPage.Content = $"{_actualPage}";
            btnPrevPage.IsEnabled = false;
            btnNextPage.IsEnabled = false;

            _currentTab = tabControl.SelectedIndex;

            pnlBottom.Visibility = Visibility.Visible;

            if (tabControl.SelectedIndex == 1)
            {
                GetHistoricItems();
                return;
            }

            GetSolicitations();
        }
    }
}
