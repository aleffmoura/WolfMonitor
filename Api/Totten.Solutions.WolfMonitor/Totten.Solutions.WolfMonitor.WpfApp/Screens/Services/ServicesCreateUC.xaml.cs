using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Items;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.SystemServices;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Services
{
    /// <summary>
    /// Interação lógica para ServicesCreateUC.xam
    /// </summary>
    public partial class ServicesCreateUC : UserControl, IItemUC
    {
        public List<string> TypeTimers { get; set; }
        private Guid _agentId;

        public ServicesCreateUC(Guid agentId)
        {
            InitializeComponent();
            _agentId = agentId;
            TypeTimers = new List<string>
            {
                new string("segundos"),
                new string("minutos")
            };
            LoadCombobox();
        }

        private void LoadCombobox() => cbTypeTime.ItemsSource = TypeTimers;

        public bool Validate()
        {
            bool isMinuts = cbTypeTime.SelectedItem.Equals("minutos");

            if (!_agentId.Equals(Guid.Empty) && !string.IsNullOrEmpty(txtName.Text) && !string.IsNullOrEmpty(txtDisplayName.Text)
                && !string.IsNullOrEmpty(txtInterval.Text) && int.TryParse(txtInterval.Text, out int result))
            {
                if (!isMinuts && int.TryParse(txtInterval.Text, out int interval) && interval < 30)
                {
                    MessageBox.Show("Minimo para intervalo é de: 30 segundos.", "Atênção", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
                return true;
            }
            return false;
        }
        public Item GetItem()
        {
            if (Validate())
            {
                return new SystemServiceCreateVO
                {
                    AgentId = _agentId,
                    Name = txtName.Text,
                    DisplayName = txtDisplayName.Text,
                    Interval = cbTypeTime.SelectedItem.Equals("minutos") ? int.Parse(txtInterval.Text) * 60 : int.Parse(txtInterval.Text),
                    Default = txtDefaultValue.Text,
                    Type = ETypeItem.SystemService
                };
            }

            return null;
        }

        public void SetItem(Item item) { }

    }
}
