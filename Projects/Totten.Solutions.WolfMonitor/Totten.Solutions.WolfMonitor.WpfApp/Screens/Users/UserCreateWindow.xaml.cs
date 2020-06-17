using System;
using System.Threading.Tasks;
using System.Windows;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.ObjectValues;
using Totten.Solutions.WolfMonitor.WpfApp.Applications;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Users;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Users
{
    public partial class UserCreateWindow : Window
    {
        private IUserService _userService;
        private TaskScheduler _currentTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private Guid _companyId;
        public UserCreateWindow(IUserService userService, Guid companyId)
        {
            InitializeComponent();
            _companyId = companyId;
            _userService = userService;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var user = InstanceUser();

            if (user == null)
                return;

            btnAdd.IsEnabled = false;

            _userService.Post(user, _companyId).ContinueWith(task =>
            {
                if (task.Result.IsFailure)
                {
                    if (SetErros(task.Result.Failure.Message))
                        MessageBox.Show("Falha no cadastro do usuário", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    else
                        MessageBox.Show(task.Result.Failure.Message, "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show("Cadastro realizado", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                }

                btnAdd.IsEnabled = true;
            }, _currentTaskScheduler);
        }

        public UserCreateVO InstanceUser()
        {
            if (!txtPass.Password.Equals(txtRepass.Password))
            {
                MessageBox.Show("As senhas não são iguais.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }


            return new UserCreateVO
            {
                Login = txtLogin.Text,
                Password = txtPass.Password,
                FirstName = txtName.Text,
                LastName = txtLastName.Text,
                Email = txtEmail.Text,
                Cpf = txtCpf.Text.Replace(".","").Replace("-",""),
                Language = "pt-BR",
            };
        }
        private bool SetErros(string msg)
        {
            ValidationErrorVO errors = new ValidationErrorVO(msg);

            lblLoginError.Content = errors["Login"];
            lblPassError.Content = errors["Password"];
            lblNameError.Content = errors["FirstName"];
            lblLastNameError.Content = errors["LastName"];
            lblEmailError.Content = errors["Email"];
            lblCpfError.Content = errors["Cpf"];

            return errors.ContainsErros;
        }
    }
}
