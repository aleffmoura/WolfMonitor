using System;
using System.ComponentModel;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Items
{
    public enum SolicitationType
    {
        [Description("Status")]
        ChangeStatus = 0,
        [Description("Chave de Configuração")]
        ChangeKeyFile = 1,
        [Description("Valor de Configuração")]
        ChangeValueFile = 2,
        [Description("Arquivo")]
        ChangeFile = 3
    }

    public class ItemSolicitationVO
    {
        public Guid ItemId { get; set; }
        public Guid AgentId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public SolicitationType SolicitationType { get; set; }
        public string NewValue { get; set; }
    }
}
