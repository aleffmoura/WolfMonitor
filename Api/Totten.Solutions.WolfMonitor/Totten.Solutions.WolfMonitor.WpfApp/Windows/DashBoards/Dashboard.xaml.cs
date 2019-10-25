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
using System.Windows.Shapes;
using Totten.Solutions.WolfMonitor.WpfApp.Windows.DashBoards.UserControls.Agents;

namespace Totten.Solutions.WolfMonitor.WpfApp.Windows.DashBoards
{
    /// <summary>
    /// Lógica interna para Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MoveSelectMenu(ListViewMenu.SelectedIndex);
        }

        private void MoveSelectMenu(int index)
        {
            TransitioningContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, 100 + (60 * index), 0, 0);
        }

        private void ListViewItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var a = new AgentsUserControl();
            GridRoot.Children.Add(a);
        }
    }
}
