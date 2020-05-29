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
        private Item _item;
        private Guid _agentId;

        public ServicesCreateUC(Guid agentId)
        {
            InitializeComponent();
            _agentId = agentId;
        }

        public bool Validate()
        {
            if (!_agentId.Equals(Guid.Empty) && !string.IsNullOrEmpty(txtName.Text) &&
                !string.IsNullOrEmpty(txtDisplayName.Text))
                return true;
            return false;
        }

        public Item GetItem()
        {
            if (Validate())
            {
                if (_item == null)
                    _item = new SystemServiceCreateVO();

                _item.AgentId = _agentId;
                _item.Name = txtName.Text;
                _item.DisplayName = txtDisplayName.Text;
                _item.AboutCurrentValue = "Item Criado";
                _item.Type = ETypeItem.SystemService;
                return _item;
            }

            return null;
        }

        public void SetItem(Item item)
        {
            txtName.Text = item.Name;
            txtDisplayName.Text = item.DisplayName;
        }
    }
}
