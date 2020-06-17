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
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Items
{
    /// <summary>
    /// Lógica interna para FrmItemsAdd.xaml
    /// </summary>
    public partial class FrmItemsAdd : Window
    {
        private UserControl _userControl;
        public Item Item { get; private set; }

        public FrmItemsAdd(UserControl userControl)
        {
            InitializeComponent();
            InsertPanel(userControl);
        }

        private void InsertPanel(UserControl userControl)
        {
            _userControl = userControl;
            rootPanel.Children.Clear();
            rootPanel.Children.Add(_userControl);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            IItemUC itemUC = _userControl as IItemUC;

            var item = itemUC.GetItem();

            if(item != null)
            {
                Item = item;
                DialogResult = true;
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Deseja cancelar?.", "Atenção",MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}
