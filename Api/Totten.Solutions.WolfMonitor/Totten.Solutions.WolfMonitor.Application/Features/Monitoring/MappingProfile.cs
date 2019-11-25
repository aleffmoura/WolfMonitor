using AutoMapper;
using Totten.Solutions.WolfMonitor.Application.Features.Monitoring.ViewModels.SystemServices;
using Totten.Solutions.WolfMonitor.Domain.Features.SystemServices;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region System Services
            //CreateMap<SystemServiceCreate.Command, SystemService>();
            //CreateMap<SystemServiceUpdate.Command, SystemService>();
            CreateMap<SystemService, SystemServiceDetailViewModel>();
            CreateMap<SystemService, SystemServiceResumeForAgentViewModel>();
            #endregion

        }
    }
}
