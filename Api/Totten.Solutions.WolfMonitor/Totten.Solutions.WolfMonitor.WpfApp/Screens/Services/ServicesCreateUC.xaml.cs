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

        public Item GetItem()
        {
            Item item = null;

            if (!_agentId.Equals(Guid.Empty) && !string.IsNullOrEmpty(txtName.Text) && !string.IsNullOrEmpty(txtDisplayName.Text)
                && !string.IsNullOrEmpty(txtInterval.Text) && int.TryParse(txtInterval.Text, out int result))
            {
                item = new SystemServiceCreateVO
                {
                    AgentId = _agentId,
                    Name = txtName.Text,
                    DisplayName = txtDisplayName.Text,
                    Interval = result,
                    Default = txtDefaultValue.Text,
                    Type = Domain.Enums.ETypeItem.SystemService
                };
                item.Interval = cbTypeTime.SelectedItem.Equals("minutos") ? item.Interval * 60 : item.Interval;
            }
            return item;
        }

        public void SetItem(Item item) { }
    }
}
