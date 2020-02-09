using AutoMapper;
using System;
using Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.Items;
using Totten.Solutions.WolfMonitor.Application.Features.Monitoring.ViewModels.SystemServices;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Items
            CreateMap<ItemCreate.Command, Item>()
                .ForMember(dest => dest.Value, src => src.MapFrom(f => "Não monitorado."))
                .ForMember(dest => dest.CreatedIn, src => src.MapFrom(f => DateTime.Now))
                .ForMember(dest => dest.UpdatedIn, src => src.MapFrom(f => DateTime.Now));


            CreateMap<ItemUpdate.Command, Item>();

            CreateMap<Item, ItemDetailViewModel>()
                .ForMember(dest=>dest.UpdatedIn,src=>src.MapFrom(item=>item.UpdatedIn.ToString("dd/MM/yyyy HH:mm:ss")));

            CreateMap<Item, ItemResumeViewModel>()
                .ForMember(dest => dest.MonitoredAt, src => src.MapFrom(item => item.MonitoredAt.HasValue ? item.MonitoredAt.Value.ToString("dd/MM/yyyy HH:mm:ss") : "Não monitorado"));

            CreateMap<Item, ItemResumeForAgentViewModel>();
            #endregion

            #region Historic Items

            CreateMap<Item, ItemHistoric>()
                .ForMember(dest => dest.Id, src => src.MapFrom(f => Guid.Empty))
                .ForMember(dest => dest.MonitoredAt, src => src.MapFrom(f => f.MonitoredAt.Value));
            #endregion

        }
    }
}
