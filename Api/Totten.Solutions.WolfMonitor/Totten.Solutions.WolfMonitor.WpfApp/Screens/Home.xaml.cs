using System.Windows;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens
{
    /// <summary>
    /// Lógica interna para Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        private UserBasicInformationViewModel _userBasicInformation;

        public Home(UserBasicInformationViewModel userBasicInformation)
        {
            InitializeComponent();
            VerifyPermissionsUser(userBasicInformation);
        }
        private void VerifyPermissionsUser(UserBasicInformationViewModel userBasicInformation)
        {
            _userBasicInformation = userBasicInformation;
            this.lblUserName.Text = _userBasicInformation.FullName;

            if(this._userBasicInformation.UserLevel >= (int)UserLevel.Admin)
            {
                this.menuItemAgents.Visibility = Visibility.Visible;
            }
            if(this._userBasicInformation.UserLevel >= (int)UserLevel.System)
            {
                this.menuItemCompanies.Visibility = Visibility.Visible;
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

    }
}
