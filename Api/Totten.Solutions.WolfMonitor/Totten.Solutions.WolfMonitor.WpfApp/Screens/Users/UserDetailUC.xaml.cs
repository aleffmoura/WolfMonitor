using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Users
{
    /// <summary>
    /// Interação lógica para UserDetailUC.xam
    /// </summary>
    public partial class UserDetailUC : UserControl
    {
        public UserDetailUC(UserBasicInformationViewModel userBasicInformation)
        {
            InitializeComponent();
            if (userBasicInformation.UserLevel < (int)EUserLevel.User)
                tbMyInfo.IsEnabled = false;
        }

        private void btnAction_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!btnAction.Content.Equals("Editar"))
            {


                btnCancel.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            btnAction.Content = "Atualizar";
            pnlUserInfo.IsEnabled = true;
            btnCancel.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            pnlUserInfo.IsEnabled = false;
            btnCancel.Visibility = System.Windows.Visibility.Collapsed;


        }
    }
}
