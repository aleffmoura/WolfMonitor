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
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.ObjectValues;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Agents;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Agents.Profiles;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Agents.Profiles
{
    /// <summary>
    /// Lógica interna para ProfileCreateWindow.xaml
    /// </summary>
    public partial class ProfileCreateWindow : Window
    {
        private TaskScheduler _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private AgentService _agentService;
        private Guid _agentId;

        public ProfileCreateWindow(AgentService agentService, Guid agentId)
        {
            InitializeComponent();
            _agentId = agentId;
            _agentService = agentService;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            btnAdd.IsEnabled = false;

            _agentService.PostProfile(new ProfileCreateVO
            {
                AgentId = _agentId,
                Name = txtName.Text
            }).ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                {
                    MessageBox.Show("Adicionado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                }
                else
                {
                    if (SetErros(msg: task.Result.Failure.Message))
                        MessageBox.Show("Ocorreram falhas", "Falha", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show(task.Result.Failure.Message, "Falha", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                btnAdd.IsEnabled = true;
            }, _taskScheduler);
        }

        private bool SetErros(string msg)
        {
            ValidationErrorVO errors = new ValidationErrorVO(msg);

            lblNameError.Content = errors["Name"];

            return errors.ContainsErros;
        }
    }
}
