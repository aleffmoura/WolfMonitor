using System.Collections.Generic;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Monitorings.NovaPasta;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Services
{
    /// <summary>
    /// Interação lógica para ServicesUserControl.xam
    /// </summary>
    public partial class ServicesUserControl : UserControl
    {
        private List<SystemServiceViewModel> _systemServiceViewModel;
        public ServicesUserControl(List<SystemServiceViewModel> systemServiceViewModel)
        {
            InitializeComponent();

            _systemServiceViewModel = systemServiceViewModel;

            Populate();
        }
        public void Populate()
        {
            this.wrapPanel.Children.Clear();
            foreach (var service in _systemServiceViewModel)
            {
                this.wrapPanel.Children.Add(new ServiceUC(service));
            }
            OnApplyTemplate();
        }
    }
}
