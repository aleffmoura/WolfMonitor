using AutoMapper;
using System;
using Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.Items;
using Totten.Solutions.WolfMonitor.Application.Features.Monitoring.ViewModels;
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
                .ForMember(dest => dest.Value, src => src.MapFrom(item => DateTime.MinValue.ToString("dd/MM/yyyy HH:mm:ss")))
                .ForMember(dest => dest.AboutCurrentValue, src => src.MapFrom(item => "Item criado."))
                .ForMember(dest => dest.Type, src => src.MapFrom(item => item.Type))
                .ForMember(dest => dest.CreatedIn, src => src.MapFrom(item => DateTime.Now))
                .ForMember(dest => dest.UpdatedIn, src => src.MapFrom(item => DateTime.Now));


            CreateMap<ItemUpdate.Command, Item>();

            CreateMap<Item, ItemDetailViewModel>()
                .ForMember(dest=>dest.UpdatedIn,src=>src.MapFrom(item=>item.UpdatedIn.ToString("dd/MM/yyyy HH:mm:ss")));

            CreateMap<Item, ItemResumeViewModel>()
                .ForMember(dest => dest.MonitoredAt, src => src.MapFrom(item => item.MonitoredAt.HasValue ? item.MonitoredAt.Value.ToString("dd/MM/yyyy HH:mm:ss") : "dd/MM/yyyy HH:mm:ss"));

            CreateMap<Item, ItemResumeForAgentViewModel>();
            #endregion

            #region Historic Items

            CreateMap<Item, ItemHistoric>()
                .ForMember(dest => dest.Id, src => src.MapFrom(item => Guid.NewGuid()))
                .ForMember(dest => dest.ItemId, src => src.MapFrom(item => item.Id))
                .ForMember(dest => dest.MonitoredAt, src => src.MapFrom(item => item.MonitoredAt.HasValue ? item.MonitoredAt.Value.ToString("dd/MM/yyyy HH:mm:ss") : DateTime.MinValue.ToString("dd/MM/yyyy HH:mm:ss")));

            CreateMap<ItemHistoric, ItemHistoric>();
            #endregion


            #region solicitations

            CreateMap<ItemSolicitationHistoric, SolicitationHistoricViewModel>()
                .ForMember(dest => dest.Value, src => src.MapFrom(item => item.NewValue))
                .ForMember(dest => dest.User, src => src.MapFrom(item => $"{item.User.FirstName} {item.User.LastName}"))
                .ForMember(dest => dest.UserEmail, src => src.MapFrom(item => item.User.Email))
                .ForMember(dest => dest.CreateAt, src => src.MapFrom(item => item.CreatedIn.ToString("dd/MM/yyyy HH:mm:ss")))
                .ForMember(dest => dest.SolicitationType, src => src.MapFrom(item => item.SolicitationType.ToString()));

            CreateMap<ItemSolicitationHistoricCreate.Command, ItemSolicitationHistoric>()
                .ForMember(dest => dest.NewValue, src => src.MapFrom(item => item.NewValue))
                .ForMember(dest => dest.CreatedIn, src => src.MapFrom(item => DateTime.Now))
                .ForMember(dest => dest.UpdatedIn, src => src.MapFrom(item => DateTime.Now));
            #endregion
        }
    }
}
