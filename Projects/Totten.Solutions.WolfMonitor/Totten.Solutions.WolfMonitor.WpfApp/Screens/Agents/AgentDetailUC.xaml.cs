using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Agents;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Agents.Profiles;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Archives;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Services;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Agents.Profiles;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Agents
{
    public partial class AgentDetailUC : UserControl
    {
        private AgentService _agentsService;
        private ItemsMonitoringService _itensMonitoringService;
        private UserBasicInformationViewModel _userBasicInformation;
        private Guid _id;

        public AgentDetailUC(Guid id, UserBasicInformationViewModel userBasicInformation,
                            AgentService agentService, ItemsMonitoringService itensMonitoringService)
        {
            InitializeComponent();
            _id = id;
            _userBasicInformation = userBasicInformation;
            _itensMonitoringService = itensMonitoringService;
            _agentsService = agentService;

            PopulateServices();
            PopulateArchives();
            InsertAgentDetail();
        }

        private void PopulateServices()
        {
            ServicesUserControl servicesUserControl = new ServicesUserControl(_id, _userBasicInformation, _itensMonitoringService, _agentsService);
            tabSystemServices.Content = servicesUserControl;
        }

        private void PopulateArchives()
        {
            ArchivesUserControl archivesUserControl = new ArchivesUserControl(_id, _userBasicInformation, _itensMonitoringService, _agentsService);
            tabSystemArchives.Content = archivesUserControl;
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

                    lblProfile.Text = task.Result.Success.ProfileName;
                }
                else
                {
                    MessageBox.Show("Falha na obtenção dos dados do agent", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void cbProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbProfile.SelectedIndex >= 0)
                btnApplyProfile.IsEnabled = true;
            else
                btnApplyProfile.IsEnabled = false;

        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            ProfileCreateWindow profileCreate = new ProfileCreateWindow(_agentsService, _id);
            profileCreate.ShowDialog();
        }

        private void btnDelProfile_Click(object sender, RoutedEventArgs e)
        {
            var serviceViewModel = cbProfile.SelectedItem as ProfileViewModel;

            if (MessageBox.Show($"Deseja realmente remover o perfil: {serviceViewModel.ProfileViewItem.Name} do agent?", "Atênção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _agentsService.DeleteProfile(_id, serviceViewModel.ProfileIdentifier).ContinueWith(task =>
                {
                    if (task.Result.IsSuccess)
                        MessageBox.Show($"Removido com sucesso", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show($"Falha na tentativa de remoção, contate o administrador", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
    }
}
