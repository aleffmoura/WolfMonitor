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
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Agents;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Services;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Agents
{
    /// <summary>
    /// Interação lógica para AgentDetailUC.xam
    /// </summary>
    public partial class AgentDetailUC : UserControl
    {
        private AgentService _agentsService;
        private ItensMonitoringService _itensMonitoringService;
        private Guid _id;

        public AgentDetailUC(Guid id, AgentService agentService, ItensMonitoringService itensMonitoringService)
        {
            InitializeComponent();
            _id = id;
            _itensMonitoringService = itensMonitoringService;
            _agentsService = agentService;
            Populate();
            InsertAgentDetail();
        }

        private void Populate()
        {
            ServicesUserControl servicesUserControl = new ServicesUserControl(_id, _itensMonitoringService);

            tabSystemServices.Content = servicesUserControl;
        }


        private async void InsertAgentDetail()
        {
           var agentCakkbak = await _agentsService.GetDetail(_id);

            if (agentCakkbak.IsSuccess)
            {
                lblName.Text = agentCakkbak.Success.DisplayName;
                lblMachineName.Text = agentCakkbak.Success.MachineName;
                lblIp.Text = agentCakkbak.Success.LocalIp;
                lblHostName.Text = agentCakkbak.Success.HostName;
                lblHostAddress.Text = agentCakkbak.Success.HostAddress;
                lblCreatedIn.Text = agentCakkbak.Success.CreatedIn;
                lblFirstConnection.Text = agentCakkbak.Success.FirstConnection;
                lblLastConnection.Text = agentCakkbak.Success.LastConnection;
                lblUpdatedIn.Text = agentCakkbak.Success.LastUpload;
                lblConfigured.Text = agentCakkbak.Success.Configured ? "Sim" : "Não";

                lblConfigured.Foreground = lblConfigured.Text.Equals("Sim") ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
            }
            else
            {

            }
        }

    }
}
