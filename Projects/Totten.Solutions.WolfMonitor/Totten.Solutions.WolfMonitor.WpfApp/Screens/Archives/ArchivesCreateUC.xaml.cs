using System;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Items;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Archives;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Archives
{
    /// <summary>
    /// Interação lógica para ArchivesCreateUC.xam
    /// </summary>
    public partial class ArchivesCreateUC : UserControl, IItemUC
    {
        private Item _item;
        private Guid _agentId;

        public ArchivesCreateUC(Guid agentId)
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
                    _item = new ArchiveCreateVO();

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
