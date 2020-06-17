using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.SystemServices;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Services
{
    /// <summary>
    /// Interação lógica para ServiceUC.xam
    /// </summary>
    public partial class ServiceUC : UserControl
    {
        private SystemServiceViewModel _systemServiceViewModel;
        private EventHandler _onRemove;
        private EventHandler _onEdit;
        private EventHandler _onRestart;
        private UserBasicInformationViewModel _userBasicInformation;

        public ServiceUC(EventHandler onRemove, EventHandler onEdit, EventHandler onRestart,
                         SystemServiceViewModel systemServiceViewModel, UserBasicInformationViewModel userBasicInformation)
        {
            InitializeComponent();
            _onRemove = onRemove;
            _onEdit = onEdit;
            _onRestart = onRestart;
            _userBasicInformation = userBasicInformation;

            if (_userBasicInformation.UserLevel < (int)EUserLevel.Admin)
            {
                btnRestart.IsEnabled = btnDel.IsEnabled = false;
                btnRestart.Visibility = btnDel.Visibility = Visibility.Collapsed;
            }

            SetServiceValues(systemServiceViewModel);
        }
        public void SetServiceValues(SystemServiceViewModel systemServiceViewModel)
        {
            _systemServiceViewModel = systemServiceViewModel;
            this.lblDisplayName.Text = _systemServiceViewModel.GetDisplayNameFormated();
            this.lblCurrentStatus.Text = _systemServiceViewModel.Value;
            this.lblServiceName.Text = _systemServiceViewModel.GetNameFormated();
            this.lblMonitoredAt.Text = _systemServiceViewModel.MonitoredAt;

            ChangeColorTextBlock(this.lblCurrentStatus);
        }

        public void ChangeColorTextBlock(TextBlock textBlock)
        {
            if (textBlock.Text.Equals(EStatusService.Running.ToString(), StringComparison.InvariantCultureIgnoreCase) ||
                textBlock.Text.Equals(EStatusService.Active.ToString(), StringComparison.InvariantCultureIgnoreCase))
                textBlock.Foreground = new SolidColorBrush(Colors.Green);
            else
                if (textBlock.Text.Equals(EStatusService.Stopped.ToString(), StringComparison.InvariantCultureIgnoreCase) ||
                    textBlock.Text.Equals(EStatusService.Inactive.ToString(), StringComparison.InvariantCultureIgnoreCase))
                textBlock.Foreground = new SolidColorBrush(Colors.Red);
            else
                textBlock.Foreground = new SolidColorBrush(Colors.Gold);
            OnApplyTemplate();
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
            => _onRemove?.Invoke(_systemServiceViewModel, new EventArgs());

        private void btnRestart_Click(object sender, RoutedEventArgs e)
            => _onRestart?.Invoke(_systemServiceViewModel, new EventArgs());

        private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e)
            => _onEdit?.Invoke(_systemServiceViewModel, new EventArgs());
    }
}
