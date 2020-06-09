using System;
using System.ComponentModel;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Features.Monitorings.VOs
{
    public enum SolicitationType
    {
        [Description("Solicitação de mudança de status")]
        ChangeStatus = 0,
        [Description("Solicitação de mudança de chave de Configuração")]
        ChangeKeyFile = 1,
        [Description("Solicitação de mudança de valor de Configuração")]
        ChangeValueFile = 2,
        [Description("Solicitação de mudança de arquivo")]
        ChangeFile = 3,
        [Description("Solicitação de mudança de valor por conter profile aplicado")]
        ChangeContainsProfile = 4
    }

    public class ItemSolicitationVO
    {
        public Guid ItemId { get; set; }
        public Guid AgentId { get; set; }
        public SolicitationType SolicitationType { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string NewValue { get; set; }
    }
}
