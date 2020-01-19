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
            _userService = new UserService(new Client.Infra.Data.Https.Base.CustomHttpCliente("http://10.0.75.1:15999"));
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var fullUserName = $"{txtUser.Text}@{txtCompany.Text}#user";
            var user = _userService.Authentication(userName: fullUserName, password: txtPass.Password);
            if (user != null)
            {
                Home home = new Home();
                home.Show();
            }
        }

        private void lblForgot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
