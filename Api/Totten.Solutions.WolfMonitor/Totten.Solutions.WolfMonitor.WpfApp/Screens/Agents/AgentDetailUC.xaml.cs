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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Agents;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Services;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Agents;

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


        private void InsertAgentDetail()
        {
            _agentsService.GetDetail(_id).ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                {
                    lblName.Text = task.Result.Success.DisplayName;
                    lblMachineName.Text = task.Result.Success.MachineName;
                    lblIp.Text = task.Result.Success.LocalIp;
                    lblHostName.Text = task.Result.Success.HostName;
                    lblHostAddress.Text = task.Result.Success.HostAddress;
                    lblCreatedIn.Text = task.Result.Success.CreatedIn;
                    lblFirstConnection.Text = task.Result.Success.FirstConnection;
                    lblLastConnection.Text = task.Result.Success.LastConnection;
                    lblConfigured.Text = task.Result.Success.Configured ? "Sim" : "Não";

                    lblConfigured.Foreground = lblConfigured.Text.Equals("Sim") ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                }
                else
                {

                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

    }
}
