using System;
using System.Windows;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Archives;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Archives
{
    /// <summary>
    /// Interação lógica para ArchiveUC.xam
    /// </summary>
    public partial class ArchiveUC : UserControl
    {
        private ArchiveViewModel _archiveViewModel;
        private EventHandler _onRemove;
        private EventHandler _onEdit;
        private UserBasicInformationViewModel _userBasicInformation;

        public ArchiveUC(EventHandler onRemove, EventHandler onEdit,
                         ArchiveViewModel archiveViewModel, UserBasicInformationViewModel userBasicInformation)
        {
            InitializeComponent();
            _onRemove = onRemove;
            _onEdit = onEdit;
            _userBasicInformation = userBasicInformation;

            if (_userBasicInformation.UserLevel < (int)EUserLevel.Admin)
            {
                btnDel.IsEnabled = btnDel.IsEnabled = false;
                btnDel.Visibility = btnDel.Visibility = Visibility.Collapsed;
            }

            SetArchiveValues(archiveViewModel);
        }

        public void SetArchiveValues(ArchiveViewModel archiveViewModel)
        {
            _archiveViewModel = archiveViewModel;
            this.lblDisplayName.Text = _archiveViewModel.GetDisplayNameFormated();
            this.lblMonitoredAt.Text = _archiveViewModel.MonitoredAt;
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
            => _onRemove?.Invoke(_archiveViewModel, new EventArgs());

        private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e)
            => _onEdit?.Invoke(_archiveViewModel, new EventArgs());
    }
}
