using AutoMapper;
using System;
using Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.SystemServices;
using Totten.Solutions.WolfMonitor.Application.Features.Monitoring.ViewModels.SystemServices;
using Totten.Solutions.WolfMonitor.Domain.Features.SystemServices;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region System Services
            CreateMap<SystemServiceCreate.Command, SystemService>()
                .ForMember(dest => dest.Value, src => src.MapFrom(f => "Not monitored yeat"))
                .ForMember(dest => dest.CreatedIn, src => src.MapFrom(f => DateTime.Now))
                .ForMember(dest => dest.UpdatedIn, src => src.MapFrom(f => DateTime.Now));
            //CreateMap<SystemServiceUpdate.Command, SystemService>();
            CreateMap<SystemService, SystemServiceDetailViewModel>();
            CreateMap<SystemService, SystemServiceResumeForAgentViewModel>();
            #endregion

        }
    }
}
