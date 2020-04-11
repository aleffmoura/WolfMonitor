using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Users;
using Totten.Solutions.WolfMonitor.WpfApp.Screens;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Passwords;

namespace Totten.Solutions.WolfMonitor.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private CustomHttpCliente _customHttp;
        private UserService _userService;
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void InstanceUserService(bool ignoreAuth = false)
        {
            _customHttp = new CustomHttpCliente("http://192.168.0.101:15999", new UserLogin
            {
                Login = $"{txtUser.Text}@{txtCompany.Text}#user",
                Password = txtPass.Password,
            }, ignoreAuth);

            _userService = new UserService(new UserEndPoint(_customHttp));
        }
        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InstanceUserService();

                UserLogin.Token = await _userService.Authentication();

                var userBasic = await _userService.GetInfo();
                if (userBasic.IsSuccess)
                {
                    Home home = new Home(_customHttp, userBasic.Success);
                    this.Visibility = Visibility.Hidden;
                    home.ShowDialog();
                }
                else
                {
                    MessageBox.Show($"Falha: {userBasic.Failure.Message}", "Atênção", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void lblForgot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            InstanceUserService(true);
            var recover = new ForgotPasswordWindow(_userService);
            recover.ShowDialog();

        }
    }
}
