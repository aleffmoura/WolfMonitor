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
            _userControl = userControl;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            IItemUC itemUC = _userControl as IItemUC;

            var item = itemUC.GetItem();
            if(item == null)
            {
                MessageBox.Show("Os dados inseridos não são validos.");
                return;
            }

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Deseja cancelar?.", "Atênção",MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}
