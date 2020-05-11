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
        private UserBasicInformationViewModel _userBasicInformation;
        public CompaniesUserControl(EventHandler onSwitchControl, UserBasicInformationViewModel userBasicInformation)
        {
            InitializeComponent();
            _onSwitchControl = onSwitchControl;
            _userBasicInformation = userBasicInformation;
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

        private void OnEdit(object sender, EventArgs e)
            => _onSwitchControl?.Invoke(new CompanyDetailUC(_userService, _userBasicInformation), new EventArgs());

        private void OnRemove(object sender, EventArgs e)
        {
            //AgentResumeViewModel agentViewModel = sender as AgentResumeViewModel;

            //if (MessageBox.Show($"Deseja realmente remover o serviço: {agentViewModel.DisplayName} do monitoramento?", "Atênção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            //{
            //    _agentService.Delete(agentViewModel.Id).ContinueWith(task =>
            //    {
            //        if (task.Result.IsSuccess)
            //        {
            //            MessageBox.Show($"Removido com sucesso", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            //            _indexes.Remove(agentViewModel.Id);
            //            PopulateByDictionary();
            //        }
            //        else
            //            MessageBox.Show($"Falha na tentativa de remoção.", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    }, TaskScheduler.FromCurrentSynchronizationContext());
            //}
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //AgentCreateWindow agentCreateWindow = new AgentCreateWindow(_agentService);
            //var result = agentCreateWindow.ShowDialog();

            //if (result.HasValue && result.Value)
            //    Populate();
        }


    }
}
