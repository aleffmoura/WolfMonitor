using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.WpfApp.Applications;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Users;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Users
{
    /// <summary>
    /// Interação lógica para UsersUserControl.xam
    /// </summary>
    public partial class UsersUserControl : UserControl
    {
        private TaskScheduler _currentTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private IUserService _userService;
        private List<UserResumeViewModel> _currentItems;
        private UserBasicInformationViewModel _userBasicInformation;
        private Guid _companyId;

        public UsersUserControl(IUserService userService,
                                UserBasicInformationViewModel userBasicInformation,
                                Guid companyId)
        {
            InitializeComponent();
            _companyId = companyId;
            _userService = userService;
            _currentItems = new List<UserResumeViewModel>();
            _userBasicInformation = userBasicInformation;

            if(_userBasicInformation.UserLevel < 2)
            {
                pnlButtons.Visibility = Visibility.Collapsed;
                pnlButtons.IsEnabled = false;
            }

            LoadUsers();
        }

        private void LoadUsers()
        {
            _userService.GetAll(_companyId).ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                {
                    _currentItems = task.Result.Success.Items;
                    gridUsers.DataContext = _currentItems;
                }
                else
                    MessageBox.Show(task.Result.Failure.Message, "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }, _currentTaskScheduler);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            UserCreateWindow userCreate = new UserCreateWindow(_userService, _companyId);

            if (userCreate.ShowDialog() == true)
            {
                txtUser.Clear();
                LoadUsers();
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            var item = (UserResumeViewModel)gridUsers.SelectedItem;

            if (item == null)
            {
                MessageBox.Show("Selecione um usuário na lista", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(item.Id == _userBasicInformation.Id)
            {
                MessageBox.Show("Não é possível remover seu próprio usuário", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            btnDel.IsEnabled = false;
            gridUsers.IsEnabled = false;

            _userService.Delete(item.Id).ContinueWith(task =>
            {
                if (task.Result.IsFailure)
                    MessageBox.Show(task.Result.Failure.Message, "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    MessageBox.Show("Deletado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    _currentItems.Remove(item);
                    SetDataOnGrid(_currentItems);
                }

                btnDel.IsEnabled = true;
                gridUsers.IsEnabled = true;

            }, _currentTaskScheduler);

        }

        private void gridUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDel.IsEnabled = false;

            if (gridUsers.SelectedItems.Count == 1)
                btnDel.IsEnabled = true;
        }

        public void SetDataOnGrid(List<UserResumeViewModel> list)
            => gridUsers.DataContext = list;

        private void btnSearch_Click(object sender, RoutedEventArgs e)
            => SetDataOnGrid(_currentItems.Where(x => x.FullName.Contains(txtUser.Text, StringComparison.OrdinalIgnoreCase) ||
                                                      x.Login.Contains(txtUser.Text, StringComparison.OrdinalIgnoreCase)).ToList());

        private void txtUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUser.Text))
                SetDataOnGrid(_currentItems);
        }
    }
}
