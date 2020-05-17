using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.WpfApp.Applications;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Companies;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Users;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Companies;

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

        public CompanyDetailUC(CompanyService companyService, IUserService userService, UserBasicInformationViewModel userBasicInformation, Guid companyId = default)
        {
            InitializeComponent();

            _companyService = companyService;
            _userService = userService;
            tbUsers.Content = new UsersUserControl(_userService);
            this._userBasicInformation = userBasicInformation;

            UsersUserControl usersUserControl = new UsersUserControl(_userService);
            tbUsers.Content = usersUserControl;

            SetValues(companyId);
        }

        private void SetValues(Guid companyId)
        {
            var id = companyId != default ? companyId : _userBasicInformation.CompanyId;

            _companyService.GetDetail(companyId).ContinueWith(task =>
            {
                var companyDetail = task.Result;

                if (companyDetail.IsSuccess)
                {
                    lblDisplayName.Text = $"{companyDetail.Success.FantasyName} / CNPJ: {companyDetail.Success.Cnpj}";
                    lblIdentifier.Text = companyDetail.Success.Id.ToString();
                    lblName.Text = companyDetail.Success.Name;
                    lblStateRegistration.Text = companyDetail.Success.StateRegistration;
                    lblMunicipalRegistration.Text = companyDetail.Success.MunicipalRegistration;

                    lblEmail.Text = companyDetail.Success.Email;
                    lblCnae.Text = companyDetail.Success.Cnae;
                    lblPhone.Text = companyDetail.Success.Phone;
                    lblAddress.Text = companyDetail.Success.Address;
                    return;
                }

                MessageBox.Show(companyDetail.Failure.Message, "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
