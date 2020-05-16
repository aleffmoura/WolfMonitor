using System;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.WpfApp.Applications;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Companies;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Users;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Companies
{
    /// <summary>
    /// Interação lógica para CompanyDetailUC.xam
    /// </summary>
    public partial class CompanyDetailUC : UserControl
    {
        private IUserService _userService;
        private CompanyService _companyService;
        private UserBasicInformationViewModel _userBasicInformation;

        public CompanyDetailUC(CompanyService companyService, IUserService userService, UserBasicInformationViewModel userBasicInformation) 
        {
            InitializeComponent();
            _companyService = companyService;
            _userService = userService;
            tbUsers.Content = new UsersUserControl(_userService);
            this._userBasicInformation = userBasicInformation;

            UsersUserControl usersUserControl = new UsersUserControl(_userService);
            tbUsers.Content = usersUserControl;

            //SetValues();
        }

        private void SetValues()
        {
            lblName.Text = "";
            lblIdentifier.Text = "";
            lblSocialReason.Text = "";
            lblStateRegistration.Text = "";
            lblMunicipalRegistration.Text = "";

            lblEmail.Text = "";
            lblCnae.Text = "";
            lblPhone.Text = "";
            lblAddress.Text = "";
        }
    }
}
