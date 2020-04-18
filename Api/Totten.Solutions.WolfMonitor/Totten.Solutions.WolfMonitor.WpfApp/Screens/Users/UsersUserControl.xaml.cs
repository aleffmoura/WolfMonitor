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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Users
{
    /// <summary>
    /// Interação lógica para UsersUserControl.xam
    /// </summary>
    public partial class UsersUserControl : UserControl
    {
        public UsersUserControl()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            UserCreateWindow userCreate = new UserCreateWindow();
            var result = userCreate.ShowDialog();

            if (result.HasValue && result.Value)
            {

            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void gridUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDel.IsEnabled = false;
        }
    }
}
