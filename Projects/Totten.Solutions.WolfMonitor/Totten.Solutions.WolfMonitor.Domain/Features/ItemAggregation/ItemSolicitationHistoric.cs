using System;
using System.ComponentModel;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

namespace Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation
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
        ChangeFile = 3,
        [Description("Solicitação de mudança de valor por conter profile aplicado")]
        ChangeContainsProfile = 4
    }
    public class ItemSolicitationHistoric : Entity
    {
        public Guid AgentId { get; set; }
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid ItemId { get; set; }

        public SolicitationType SolicitationType { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string NewValue { get; set; }

        public virtual User User { get; set; }

        public override void Validate()
        { }
    }
}
