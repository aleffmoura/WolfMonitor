using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Authentication;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Monitorings;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Agents;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Users;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Agents;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Companies;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens
{
    public partial class Home : Window
    {
        private UserBasicInformationViewModel _userBasicInformation;
        private CustomHttpCliente _customHttpCliente;

        public Home(CustomHttpCliente customHttpCliente,
                    UserBasicInformationViewModel userBasicInformation)
        {
            InitializeComponent();
            _customHttpCliente = customHttpCliente;
            VerifyPermissionsUser(userBasicInformation);
        }

        private void IncludeUserControl(object sender, EventArgs e)
        {
            UserControl userControl = (UserControl)sender;

            gridRoot.Children.Clear();

            gridRoot.Children.Add(userControl);
        }

        private void VerifyPermissionsUser(UserBasicInformationViewModel userBasicInformation)
        {
            _userBasicInformation = userBasicInformation;
            this.lblUserName.Text = _userBasicInformation.FullName;

            if (this._userBasicInformation.UserLevel < (int)UserLevel.System)
            {
                this.btnCompanyMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
            => Application.Current.Shutdown();

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


        private void viewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = viewMenu.SelectedIndex;
            MoveHighlighter(index);
        }

        private void MoveHighlighter(int index)
        {
            transitionHighlighter.OnApplyTemplate();
            gridHighlighter.Margin = new Thickness(0, 60 + (60 * index), 0, 0);
        }

        private void btnAgentsMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var service = new AgentService(new AgentEndPoint(_customHttpCliente));
            var monitoringItems = new ItemsMonitoringService(new ItemsEndPoint(_customHttpCliente));

            var agentsUserControl = new AgentsUserControl(service, monitoringItems, IncludeUserControl);

            IncludeUserControl(agentsUserControl, new EventArgs());
            agentsUserControl.Populate();
        }

        private void btnCompanyMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var company = new CompanyDetailUC(new UserService(new UserEndPoint(_customHttpCliente)));

            IncludeUserControl(company, new EventArgs());
        }
    }
}
