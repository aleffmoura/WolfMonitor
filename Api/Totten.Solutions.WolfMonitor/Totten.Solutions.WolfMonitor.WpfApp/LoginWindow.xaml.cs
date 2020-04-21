using System;
using System.Windows;
using System.Windows.Input;
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
            pnlRoot.IsEnabled = false;

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
                    MessageBox.Show($"Falha: {userBasic.Failure.Message}", "Atênção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha: {ex.Message}", "Atênção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                pnlRoot.IsEnabled = true;
            }
        }

        private void lblForgot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pnlRoot.IsEnabled = false;

            InstanceUserService(true);

            try
            {
                var recover = new ForgotPasswordWindow(_userService);
                recover.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Falha inesperada, por favor contate um administrador de sistema", "Atênção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            pnlRoot.IsEnabled = true;
        }
    }
}
