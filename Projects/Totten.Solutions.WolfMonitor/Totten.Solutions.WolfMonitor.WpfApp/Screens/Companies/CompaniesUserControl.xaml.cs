using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.WpfApp.Applications;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Companies;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Companies;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Companies
{
    /// <summary>
    /// Interação lógica para CompaniesUserControl.xam
    /// </summary>
    public partial class CompaniesUserControl : UserControl
    {
        private Dictionary<Guid, CompanyUC> _indexes;
        private EventHandler _onSwitchControl;
        private IUserService _userService;
        private CompanyService _companyService;
        private UserBasicInformationViewModel _userBasicInformation;

        public CompaniesUserControl(CompanyService companyService,
                                    EventHandler onSwitchControl,
                                    IUserService userService,
                                    UserBasicInformationViewModel userBasicInformation)
        {
            InitializeComponent();
            _onSwitchControl = onSwitchControl;
            _companyService = companyService;
            _userService = userService;
            _userBasicInformation = userBasicInformation;
            _indexes = new Dictionary<Guid, CompanyUC>();
        }

        ~CompaniesUserControl()
        {
            _indexes.Clear();
            _indexes = null;
        }

        public void PopulateByDictionary()
        {
            this.wrapPanel.Children.Clear();

            foreach (var companyViewModel in _indexes)
                this.wrapPanel.Children.Add(_indexes[companyViewModel.Key]);

            OnApplyTemplate();
        }


        public void Populate()
        {
            _indexes.Clear();

            this.wrapPanel.Children.Clear();

            var loading = new LoadingWindow(_companyService.GetAll().ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                {
                    foreach (CompanyResumeViewModel companyViewModel in task.Result.Success.Items)
                    {
                        _indexes.Add(companyViewModel.Id, new CompanyUC(OnRemove, OnEdit, companyViewModel, _userBasicInformation));
                    }
                    PopulateByDictionary();
                }
                else
                    MessageBox.Show("Falha na requisição de empresas", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
            }, TaskScheduler.FromCurrentSynchronizationContext()));

            loading.ShowDialog();
        }

        private void OnEdit(object sender, EventArgs e)
            => _onSwitchControl?.Invoke(new CompanyDetailUC(_companyService, _userService, _userBasicInformation, (Guid)sender), new EventArgs());

        private void OnRemove(object sender, EventArgs e)
        {
           CompanyResumeViewModel companyViewModel = sender as CompanyResumeViewModel;

            if (MessageBox.Show($"Deseja realmente remover a empresa: {companyViewModel.Company}?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _companyService.Delete(companyViewModel.Id).ContinueWith(task =>
                {
                    if (task.Result.IsSuccess)
                    {
                        MessageBox.Show($"Removido com sucesso", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                        _indexes.Remove(companyViewModel.Id);
                        PopulateByDictionary();
                    }
                    else
                        MessageBox.Show($"Falha na tentativa de remoção.", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CompanyCreateWindow createWindow = new CompanyCreateWindow(_companyService);

            var result = createWindow.ShowDialog();

            if (result.HasValue && result.Value)
                Populate();
        }

        public void SetDataOnGrid(List<KeyValuePair<Guid, CompanyUC>> list)
        {
            this.wrapPanel.Children.Clear();

            foreach (var companyView in list)
                this.wrapPanel.Children.Add(_indexes[companyView.Key]);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
         => SetDataOnGrid(_indexes.Where(x => x.Value.lblCompanyName.Text.Contains(txtCompanyName.Text, StringComparison.OrdinalIgnoreCase)).ToList());


        private void txtCompanyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCompanyName.Text))
                SetDataOnGrid(_indexes.ToList());
        }
    }
}
