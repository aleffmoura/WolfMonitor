using System;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.ViewModels
{
    public class SolicitationHistoricViewModel
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public string UserEmail { get; set; }
        public string SolicitationType { get; set; }
        public string Value { get; set; }
        public string CreateAt { get; set; }
    }
}
