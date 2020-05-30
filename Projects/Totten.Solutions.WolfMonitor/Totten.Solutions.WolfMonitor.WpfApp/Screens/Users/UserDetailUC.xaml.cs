using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Users;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Users
{
    public partial class UserDetailUC : UserControl
    {
        private UserService _userService;
        private UserBasicInformationViewModel _userBasicInformation;

        public UserDetailUC(UserService userService, UserBasicInformationViewModel userBasicInformation)
        {
            InitializeComponent();
            _userBasicInformation = userBasicInformation;

            if (userBasicInformation.UserLevel < (int)EUserLevel.User)
                tbMyInfo.IsEnabled = false;

            _userService = userService;

            SetValues();

            _userService.GetAllAgentsByUser().ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                    gridAgends.ItemsSource = task.Result.Success.Items;
                else
                    MessageBox.Show("Falha na obtenção dos agents", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void SetValues()
        {
            txtLogin.Text = _userBasicInformation.Login;
            txtPass.Password = _userBasicInformation.Password;
            txtRepass.Password = _userBasicInformation.Password;

            txtEmail.Text = _userBasicInformation.Email;
            txtName.Text = _userBasicInformation.FirstName;
            txtLastName.Text = _userBasicInformation.LastName;
            txtCpf.Text = _userBasicInformation.Cpf;
        }

        private bool VerifyChanges()
        {
            return txtLogin.Text != _userBasicInformation.Login ||
            txtPass.Password != _userBasicInformation.Password ||
            txtRepass.Password != _userBasicInformation.Password ||
            txtEmail.Text != _userBasicInformation.Email ||
            txtName.Text != _userBasicInformation.LastName ||
            txtLastName.Text != _userBasicInformation.LastName ||
            txtCpf.Text != _userBasicInformation.Cpf;
        }


        private void btnAction_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!btnAction.Content.Equals("Editar"))
            {
                btnAction.Content = "Editar";
                btnAction.IsEnabled = true;
                btnCancel.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            btnAction.Content = "Atualizar";
            btnAction.IsEnabled = false;

            pnlUserInfo.IsEnabled = true;
            btnCancel.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            pnlUserInfo.IsEnabled = false;
            btnAction.Content = "Editar";
            btnAction.IsEnabled = true;
            btnCancel.Visibility = System.Windows.Visibility.Collapsed;
            SetValues();
        }

        private void textChanged(object sender, EventArgs e)
        {
            if (btnAction.Content.Equals("Atualizar"))
            {
                if (VerifyChanges())
                {
                    btnAction.IsEnabled = true;
                    return;
                }
                btnAction.IsEnabled = false;
            }
        }

    }
}
