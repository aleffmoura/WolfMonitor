using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens
{
    /// <summary>
    /// Lógica interna para LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window, IDisposable
    {
        private Task _work;

        public LoadingWindow(Task work)
        {
            InitializeComponent();
            _work = work;
        }

        public void Dispose()
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_work != null)
                _work.ContinueWith(task => this.Close(), TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
