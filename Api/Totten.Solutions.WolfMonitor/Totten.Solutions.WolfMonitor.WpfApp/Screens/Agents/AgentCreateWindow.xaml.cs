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
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Agents;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Agents;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Agents
{
    /// <summary>
    /// Lógica interna para AgentCreateWindow.xaml
    /// </summary>
    public partial class AgentCreateWindow : Window
    {
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
                MessageBox.Show("Todos os campos são obrigatórios.", "Atênção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _agentService.Post(new AgentCreateVO
            {
                DisplayName = txtName.Text,
                Login = txtUser.Text,
                Password = txtPass.Password
            }).ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                    MessageBox.Show("Adicionado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Falha na tentativa de adicionar um agent.", "Falha", MessageBoxButton.OK, MessageBoxImage.Information);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
