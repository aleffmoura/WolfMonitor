using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Totten.Solutions.WolfMonitor.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private UserService _userService;
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var custom = new CustomHttpCliente("http://10.0.75.1:15999", new UserLogin
                {
                    Login = $"{txtUser.Text}@{txtCompany.Text}#user",
                    Password = txtPass.Password,
                });
                _userService = new UserService(new UserEndPoint(custom));

                UserLogin.Token = _userService.Authentication();
                _userService.GetInfo().ContinueWith(task =>
                {
                    if (task.Result.IsSuccess)
                    {
                        Home home = new Home(custom, task.Result.Success);
                        this.Visibility = Visibility.Hidden;
                        home.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show($"Falha: {task.Result.Failure.Message}", "Atênção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());

            }
            catch(Exception ex)
            {
            }
        }

        private void lblForgot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
