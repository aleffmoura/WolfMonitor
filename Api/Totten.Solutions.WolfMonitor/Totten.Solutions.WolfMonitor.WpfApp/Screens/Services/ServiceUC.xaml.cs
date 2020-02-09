using System;
using System.Windows.Controls;
using System.Windows.Media;
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

        public ServiceUC(EventHandler onRemove, EventHandler onEdit, SystemServiceViewModel systemServiceViewModel)
        {
            InitializeComponent();
            _onRemove = onRemove;
            _onEdit = onEdit;
            _systemServiceViewModel = systemServiceViewModel;
            SetServiceValues();
        }

        public void SetServiceValues()
        {
            this.lblDisplayName.Text = _systemServiceViewModel.DisplayName;
            this.lblCurrentStatus.Text = _systemServiceViewModel.Value;
            this.lblLastStatus.Text = _systemServiceViewModel.LastStatus;
            this.lblServiceName.Text = _systemServiceViewModel.Name;
            this.lblMonitoredAt.Text = _systemServiceViewModel.MonitoredAt;

            ChangeColorTextBlock(this.lblCurrentStatus);
            ChangeColorTextBlock(this.lblLastStatus);
        }

        public void ChangeColorTextBlock(TextBlock textBlock)
        {
            if(textBlock.Text.Equals("running", System.StringComparison.InvariantCultureIgnoreCase))
                textBlock.Foreground = new SolidColorBrush(Colors.Green);
            else if(textBlock.Text.Equals("stopped", System.StringComparison.InvariantCultureIgnoreCase))
                textBlock.Foreground = new SolidColorBrush(Colors.Red);
            else
                textBlock.Foreground = new SolidColorBrush(Colors.Gold);
            OnApplyTemplate();
        }

        private void btnDel_Click(object sender, System.Windows.RoutedEventArgs e)
            => _onRemove?.Invoke(_systemServiceViewModel, new EventArgs());

        private void btnRestart_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ServiceDetailWindow serviceDetail = new ServiceDetailWindow();
            serviceDetail.ShowDialog();
        }
    }
}
