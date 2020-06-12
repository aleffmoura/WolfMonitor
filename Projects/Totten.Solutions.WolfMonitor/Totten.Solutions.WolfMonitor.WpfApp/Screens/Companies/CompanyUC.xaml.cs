using System;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Companies;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Companies
{
    /// <summary>
    /// Interação lógica para CompanyUC.xam
    /// </summary>
    public partial class CompanyUC : UserControl
    {
        private EventHandler _onRemove;
        private EventHandler _onEditHandler;
        private UserBasicInformationViewModel _userBasicInformation;
        private CompanyResumeViewModel _companyResumeViewModel;

        public CompanyUC(EventHandler onRemove, EventHandler onEdit, CompanyResumeViewModel companyResumeViewModel, UserBasicInformationViewModel userBasicInformation)
        {
            InitializeComponent();
            _companyResumeViewModel = companyResumeViewModel;
            _onRemove = onRemove;
            _onEditHandler = onEdit;
            _userBasicInformation = userBasicInformation;
            SetCompanyValues();
        }

        ~CompanyUC()
        {
            _onRemove = null;
            _onEditHandler = null;
            _companyResumeViewModel = null;
        }

        public void SetCompanyValues()
        {
            lblCompanyName.Text = _companyResumeViewModel.GetDisplayNameFormated(); 
            lblQtAgents.Text = $"{_companyResumeViewModel.QtdAgents}";
            lblQtServices.Text = $"{_companyResumeViewModel.QtdServices}";
            lblQtArchives.Text = $"{_companyResumeViewModel.QtdArchives}";
            lblQtUsers.Text = $"{_companyResumeViewModel.QtdUsers}";
        }

        private void btnDel_Click(object sender, System.Windows.RoutedEventArgs e)
            => _onRemove?.Invoke(_companyResumeViewModel, new EventArgs());

        private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e)
            => _onEditHandler?.Invoke(_companyResumeViewModel.Id, new EventArgs());
    }
}
