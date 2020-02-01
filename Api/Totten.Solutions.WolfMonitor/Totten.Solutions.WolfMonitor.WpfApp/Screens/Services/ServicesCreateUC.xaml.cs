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

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Services
{
    /// <summary>
    /// Interação lógica para ServicesCreateUC.xam
    /// </summary>
    public partial class ServicesCreateUC : UserControl
    {
        public List<string> Items { get; set; }
        public ServicesCreateUC()
        {
            InitializeComponent();
            Items = new List<string>
            {
                new string("segundos"),
                new string("minutos")
            };
            LoadCombobox();
        }

        private void LoadCombobox()
        {
            cbTypeTime.ItemsSource = Items;
        }
    }
}
