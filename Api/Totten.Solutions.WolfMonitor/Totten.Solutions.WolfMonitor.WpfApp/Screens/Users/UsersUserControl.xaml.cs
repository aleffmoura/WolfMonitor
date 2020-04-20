using System;
using System.Collections.Generic;
using System.Linq;
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

        public UsersUserControl(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
            _currentItems = new List<UserResumeViewModel>();
            LoadUsers();
        }

        private void LoadUsers()
        {
            _userService.GetAll().ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                {
                    _currentItems = task.Result.Success.Items;
                    gridUsers.DataContext = _currentItems;
                }
                else
                    MessageBox.Show(task.Result.Failure.Message, "Atênção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }, _currentTaskScheduler);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            UserCreateWindow userCreate = new UserCreateWindow(_userService);
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
                MessageBox.Show("Selecione um usuário na lista", "Atênção", MessageBoxButton.OK, MessageBoxImage.Warning);
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
