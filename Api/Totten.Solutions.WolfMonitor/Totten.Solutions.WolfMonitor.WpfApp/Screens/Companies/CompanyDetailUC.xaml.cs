using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.WpfApp.Applications;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Users;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Companies
{
    /// <summary>
    /// Interação lógica para CompanyDetailUC.xam
    /// </summary>
    public partial class CompanyDetailUC : UserControl
    {
        private IUserService _userService;
        private UserBasicInformationViewModel _userBasicInformation;

        public CompanyDetailUC(IUserService userService, UserBasicInformationViewModel userBasicInformation) 
        {
            InitializeComponent();

            _userService = userService;
            tbUsers.Content = new UsersUserControl(_userService);
            this._userBasicInformation = userBasicInformation;
        }
    }
}
