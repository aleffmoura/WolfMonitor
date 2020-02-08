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
            #region System Services
            CreateMap<ItemCreate.Command, Item>()
                .ForMember(dest => dest.Value, src => src.MapFrom(f => "Não monitorado."))
                .ForMember(dest => dest.CreatedIn, src => src.MapFrom(f => DateTime.Now))
                .ForMember(dest => dest.UpdatedIn, src => src.MapFrom(f => DateTime.Now));

            CreateMap<ItemUpdate.Command, Item>();

            CreateMap<Item, ItemDetailViewModel>();

            CreateMap<Item, ItemResumeViewModel>();

            CreateMap<Item, ItemResumeForAgentViewModel>();
            #endregion

        }
    }
}
