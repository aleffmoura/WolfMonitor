using System;
using System.Collections.Generic;
using System.Windows;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Monitorings;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Monitorings.NovaPasta;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Services;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens
{
    /// <summary>
    /// Lógica interna para Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        private UserBasicInformationViewModel _userBasicInformation;
        private CustomHttpCliente _customHttpCliente;

        public Home(CustomHttpCliente customHttpCliente, UserBasicInformationViewModel userBasicInformation)
        {
            InitializeComponent();
            _customHttpCliente = customHttpCliente;
            VerifyPermissionsUser(userBasicInformation);
        }
        private void VerifyPermissionsUser(UserBasicInformationViewModel userBasicInformation)
        {
            _userBasicInformation = userBasicInformation;
            this.lblUserName.Text = _userBasicInformation.FullName;

            if (this._userBasicInformation.UserLevel < (int)UserLevel.Admin)
            {
                this.menuItemAgents.Visibility = Visibility.Collapsed;
            }
            if (this._userBasicInformation.UserLevel < (int)UserLevel.System)
            {
                this.menuItemCompanies.Visibility = Visibility.Collapsed;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            btnOpenMenu.Visibility = Visibility.Collapsed;
            btnCloseMenu.Visibility = Visibility.Visible;
        }

        private void btnCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            btnOpenMenu.Visibility = Visibility.Visible;
            btnCloseMenu.Visibility = Visibility.Collapsed;
        }

        private async void btnAgentsMenu_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ItensMonitoringService itensMonitoringService = new ItensMonitoringService(new SystemServiceEndPoint(_customHttpCliente));

            var callBack = await itensMonitoringService.GetSystemServices();

            ServicesUserControl servicesUserControl = new ServicesUserControl(callBack.Success.Items);
            gridRoot.Children.Clear();
            gridRoot.Children.Add(servicesUserControl);
        }

        private void viewMenu_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var index = viewMenu.SelectedIndex;
            MoveHighlighter(index);
        }

        private void MoveHighlighter(int index)
        {
            transitionHighlighter.OnApplyTemplate();
            gridHighlighter.Margin = new Thickness(0, 60 + (60 * index), 0, 0);
        }
    }
}
