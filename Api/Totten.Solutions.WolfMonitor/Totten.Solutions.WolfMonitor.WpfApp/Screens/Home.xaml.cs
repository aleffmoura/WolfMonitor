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
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Users;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens
{
    public partial class Home : Window
    {
        private UserBasicInformationViewModel _userBasicInformation;
        private CustomHttpCliente _customHttpCliente;
        private AgentService _agentService;
        private ItemsMonitoringService _itemsMonitoringService;
        private UserService _userService;

        public Home(CustomHttpCliente customHttpCliente,
                    UserBasicInformationViewModel userBasicInformation)
        {
            InitializeComponent();

            _customHttpCliente = customHttpCliente;

            InstanceServices();

            VerifyPermissionsUser(userBasicInformation);
        }

        private void InstanceServices()
        {
            _agentService = new AgentService(new AgentEndPoint(_customHttpCliente));
            _itemsMonitoringService = new ItemsMonitoringService(new ItemsEndPoint(_customHttpCliente));
            _userService = new UserService(new UserEndPoint(_customHttpCliente));
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

            if (this._userBasicInformation.UserLevel < (int)EUserLevel.System)
                this.btnCompanyMenu.Visibility = Visibility.Collapsed;
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
            var agentsUserControl = new AgentsUserControl(_agentService, _itemsMonitoringService, IncludeUserControl, _userBasicInformation);

            IncludeUserControl(agentsUserControl, new EventArgs());
            agentsUserControl.Populate();
        }

        private void btnCompanyMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            => IncludeUserControl(new CompanyDetailUC(_userService, _userBasicInformation), new EventArgs());
        

        private void btnMyAccount_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            => IncludeUserControl(new UserDetailUC(_userService, _userBasicInformation), new EventArgs());
    }
}
