using System;
using System.Windows.Controls;
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

        public AgentUC(EventHandler onRemove, AgentResumeViewModel agentResumeViewModel)
        {
            InitializeComponent();
            _agentResumeViewModel = agentResumeViewModel;
            _onRemove = onRemove;
            SetServiceValues();
        }
        ~AgentUC()
        {
            _onRemove = null;
            _agentResumeViewModel = null;
        }
        private void btnDel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _onRemove?.Invoke(_agentResumeViewModel, new EventArgs());
        }

        public async void SetServiceValues()
        {
            lblDisplayName.Text = _agentResumeViewModel.DisplayName;
            lblCreatedBy.Text = _agentResumeViewModel.UserWhoCreated;
            lblCreatedIn.Text = _agentResumeViewModel.CreatedIn;
            lblUpdatedIn.Text = _agentResumeViewModel.LastUpdate;
            lblServicesCount.Text = $"{TryGetCount(ETypeItem.SystemService)}";
            lblConfigsCount.Text = $"{TryGetCount(ETypeItem.SystemConfig)}";
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
