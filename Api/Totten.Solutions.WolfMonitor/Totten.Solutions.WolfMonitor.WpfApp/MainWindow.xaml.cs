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

namespace Totten.Solutions.WolfMonitor.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserService _userService;
        public MainWindow()
        {
            InitializeComponent();
            _userService = new UserService(new Client.Infra.Data.Https.Base.CustomHttpCliente("http://10.0.75.1:15999"));
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            _userService.Authentication(userName: txtUser.Text, password: txtPass.Password);
        }

        private void lblForgot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
