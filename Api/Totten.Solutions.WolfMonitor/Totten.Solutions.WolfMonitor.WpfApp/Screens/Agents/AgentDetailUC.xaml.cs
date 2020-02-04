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
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Services;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Agents
{
    /// <summary>
    /// Interação lógica para AgentDetailUC.xam
    /// </summary>
    public partial class AgentDetailUC : UserControl
    {
        private ItensMonitoringService _itensMonitoringService;
        private Guid _id;

        public AgentDetailUC(Guid id, ItensMonitoringService itensMonitoringService)
        {
            InitializeComponent();
            _id = id;
            _itensMonitoringService = itensMonitoringService;
            Populate();
        }

        private void Populate()
        {
            ServicesUserControl servicesUserControl = new ServicesUserControl(_id, _itensMonitoringService);

            tabSystemServices.Content = servicesUserControl;
        }
    }
}
