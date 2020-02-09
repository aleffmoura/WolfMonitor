using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.SystemServices;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Services
{
    /// <summary>
    /// Lógica interna para ServiceDetailWindow.xaml
    /// </summary>
    public partial class ServiceDetailWindow : Window
    {
        private SystemServiceViewModel _systemServiceView;

        public ServiceDetailWindow(SystemServiceViewModel systemServiceView)
        {
            InitializeComponent();
            _systemServiceView = systemServiceView;
            Populate();
        }

        private void Populate()
        {
            this.lblDisplayName.Text = _systemServiceView.DisplayName;
            this.lblName.Text = _systemServiceView.Name;
            this.lblCurrentValue.Text = _systemServiceView.Value;
            this.lblMonitoredAt.Text = _systemServiceView.MonitoredAt;
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
