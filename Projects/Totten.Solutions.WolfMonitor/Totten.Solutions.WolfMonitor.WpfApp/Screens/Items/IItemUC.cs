
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Items
{
    interface IItemUC
    {
        bool Validate();
        Item GetItem();
        void SetItem(Item item);
    }
}
