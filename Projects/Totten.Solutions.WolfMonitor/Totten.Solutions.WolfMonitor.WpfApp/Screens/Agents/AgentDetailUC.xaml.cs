using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Agents;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Agents.Profiles;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Archives;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Services;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Agents;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Agents.Profiles;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Agents
{
    public partial class AgentDetailUC : UserControl
    {
        private AgentService _agentsService;
        private ItemsMonitoringService _itensMonitoringService;
        private UserBasicInformationViewModel _userBasicInformation;
        private AgentDetailViewModel _agentDetail;
        private Guid _id;

        public AgentDetailUC(Guid id, UserBasicInformationViewModel userBasicInformation,
                            AgentService agentService, ItemsMonitoringService itensMonitoringService)
        {
            InitializeComponent();
            _id = id;
            _userBasicInformation = userBasicInformation;
            _itensMonitoringService = itensMonitoringService;
            _agentsService = agentService;

            if(_userBasicInformation.UserLevel < (int)EUserLevel.Admin)
                this.pnlProfile.IsEnabled = false;

            PopulateServices();
            PopulateArchives();
            InsertAgentDetail().ContinueWith(task =>
            {
                PopulateCbProfile();
            }, TaskScheduler.FromCurrentSynchronizationContext());
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

        private void PopulateCbProfile()
        {
            _agentsService.GetAllProfiles(_id).ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                {
                    var grouped = task.Result.Success.Items.GroupBy(x => x.Name);

                    var source = new List<ProfileViewModel>
                    {
                        new ProfileViewModel
                        {
                            Name = "Sem perfil",
                            ProfileViewItem = new List<ProfileViewItem>
                            {
                                new ProfileViewItem
                                {
                                    AgentId = _id,
                                    Name = "Sem perfil",
                                    ProfileIdentifier = Guid.Empty
                                }
                            }
                        }
                    };

                    foreach (var item in grouped)
                    {
                        source.Add(new ProfileViewModel
                        {
                            Name = item.Key,
                            ProfileViewItem = item.ToList()
                        });
                    }

                    cbProfile.ItemsSource = source;
                    cbProfile.DisplayMemberPath = "Name";

                    var profile = source.FirstOrDefault(x => x.Name.Equals(_agentDetail.ProfileName));

                    if (profile != null)
                        cbProfile.SelectedItem = profile;
                }
                else
                {
                    MessageBox.Show("Falha na obtenção dos perfis do agent", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private Task InsertAgentDetail()
        {
            return _agentsService.GetDetail(_id).ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                {
                    _agentDetail = task.Result.Success;

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
                    MessageBox.Show("Falha na obtenção do detalhamento do agent", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void cbProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbProfile.SelectedIndex > 0)
                btnDelProfile.IsEnabled = btnApplyProfile.IsEnabled = true;
            else if (cbProfile.SelectedIndex == 0)
            {
                btnApplyProfile.IsEnabled = true;
                btnDelProfile.IsEnabled = false;
            }
            else
                btnDelProfile.IsEnabled = btnApplyProfile.IsEnabled = false;

        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            ProfileCreateWindow profileCreate = new ProfileCreateWindow(_agentsService, _id);
            
            if(profileCreate.ShowDialog() == true)
                PopulateCbProfile();
        }

        private void btnDelProfile_Click(object sender, RoutedEventArgs e)
        {
            var serviceViewModel = cbProfile.SelectedItem as ProfileViewModel;

            if (MessageBox.Show($"Deseja realmente remover o perfil: {serviceViewModel.Name} do agent?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _agentsService.DeleteProfile(_id, serviceViewModel.ProfileViewItem.FirstOrDefault().ProfileIdentifier).ContinueWith(task =>
                {
                    if (task.Result.IsSuccess)
                    {
                        MessageBox.Show($"Removido com sucesso", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                        var list = cbProfile.ItemsSource as List<ProfileViewModel>;

                        list.Remove(serviceViewModel);

                        if (serviceViewModel.Name.Equals(_agentDetail.ProfileName))
                        {
                            cbProfile.SelectedIndex = 0;
                            _agentDetail.ProfileName = cbProfile.Text;
                            lblProfile.Text = _agentDetail.ProfileName;
                        }
                    }
                    else
                        MessageBox.Show($"Falha na tentativa de remoção, contate o administrador", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }

        }

        private void btnApplyProfile_Click(object sender, RoutedEventArgs e)
        {
            var profileView = cbProfile.SelectedItem as ProfileViewModel;

            var profileApply = new ProfileApplyVO
            {
                AgentId = _id,
                ProfileIdentifier = profileView.ProfileViewItem[0].ProfileIdentifier
            };

            _agentsService.ApplyProfile(profileApply).ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                {
                    MessageBox.Show($"Foi aplicado o perfil com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    lblProfile.Text = profileView.Name;
                }
                else
                    MessageBox.Show($"Falha na tentativa de aplicar o perfil, contate o administrador", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
