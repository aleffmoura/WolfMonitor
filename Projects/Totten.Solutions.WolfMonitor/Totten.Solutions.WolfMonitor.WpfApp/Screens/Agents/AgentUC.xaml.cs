using System;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Agents;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Agents
{
    /// <summary>
    /// Interação lógica para AgentUC.xam
    /// </summary>
    public partial class AgentUC : UserControl
    {
        private AgentResumeViewModel _agentResumeViewModel;
        private EventHandler _onRemove;
        private EventHandler _onEditHandler;
        private UserBasicInformationViewModel _userBasicInformation;

        public AgentUC(EventHandler onRemove, EventHandler onEdit,
                       AgentResumeViewModel agentResumeViewModel, UserBasicInformationViewModel userBasicInformation)
        {
            InitializeComponent();
            _agentResumeViewModel = agentResumeViewModel;
            _onRemove = onRemove;
            _onEditHandler = onEdit;
            _userBasicInformation = userBasicInformation;
            SetAgentValues();
        }

        ~AgentUC()
        {
            _onRemove = null;
            _onEditHandler = null;
            _agentResumeViewModel = null;
        }

        private void btnDel_Click(object sender, System.Windows.RoutedEventArgs e)
            => _onRemove?.Invoke(_agentResumeViewModel, new EventArgs());

        private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e)
            => _onEditHandler?.Invoke(_agentResumeViewModel.Id, new EventArgs());

        public void SetAgentValues()
        {
            lblDisplayName.Text = _agentResumeViewModel.GetDisplayNameFormated();
            lblCreatedBy.Text = _agentResumeViewModel.UserWhoCreated;
            lblCreatedIn.Text = _agentResumeViewModel.CreatedIn;
            lblUpdatedIn.Text = _agentResumeViewModel.LastUpdate;
            lblServicesCount.Text = $"{TryGetCount(ETypeItem.SystemService)}";
            lblConfigsCount.Text = $"{TryGetCount(ETypeItem.SystemConfig)}";

            if (_userBasicInformation.UserLevel == (int)EUserLevel.User)
            {
                btnDel.IsEnabled =  false;
                btnDel.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private int TryGetCount(ETypeItem eTypeItem)
        {
            int returned = 0;

            if (_agentResumeViewModel.Items.TryGetValue((int)eTypeItem, out int count))
            {
                returned = count;
            }
            return returned;
        }

    }
}
