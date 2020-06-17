using System.Threading.Tasks;
using System.Windows;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.ObjectValues;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Agents;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Agents;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Agents
{
    /// <summary>
    /// Lógica interna para AgentCreateWindow.xaml
    /// </summary>
    public partial class AgentCreateWindow : Window
    {
        private TaskScheduler _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private AgentService _agentService;
        public AgentCreateWindow(AgentService agentService)
        {
            InitializeComponent();
            _agentService = agentService;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtPass.Password))
            {
                MessageBox.Show("Todos os campos são obrigatórios.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            btnAdd.IsEnabled = false;

            _agentService.Post(new AgentCreateVO
            {
                DisplayName = txtName.Text,
                Login = txtUser.Text,
                Password = txtPass.Password
            }).ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                {
                    MessageBox.Show("Adicionado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                }
                else
                {
                    if(SetErros(msg: task.Result.Failure.Message))
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

            lblNameError.Content = errors["DisplayName"];
            lblUserError.Content = errors["Login"];
            lblPassError.Content = errors["Password"];

            return errors.ContainsErros;
        }
    }
}
