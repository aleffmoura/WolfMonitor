using AutoMapper;
using System;
using Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers;
using Totten.Solutions.WolfMonitor.Application.Features.Agents.ViewModels;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;

namespace Totten.Solutions.WolfMonitor.Application.Features.Agents
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AgentCreate.Command, Agent>()
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.UserWhoCreatedId, opt => opt.MapFrom(src => src.UserWhoCreatedId))
                .ForMember(dest => dest.UserWhoCreatedName, opt => opt.MapFrom(src => src.UserWhoCreatedName))
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.MachineName, opt => opt.MapFrom(src => "Nunca conectado"))
                .ForMember(dest => dest.LocalIp, opt => opt.MapFrom(src => "Nunca conectado"))
                .ForMember(dest => dest.HostName, opt => opt.MapFrom(src => "Nunca conectado"))
                .ForMember(dest => dest.HostAddress, opt => opt.MapFrom(src => "Nunca conectado"))
                .ForMember(dest => dest.CreatedIn, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedIn, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.FirstConnection, opt => opt.MapFrom(src => (DateTime?)null))
                .ForMember(dest => dest.LastConnection, opt => opt.MapFrom(src => (DateTime?)null))
                .ForMember(dest => dest.LastUpload, opt => opt.MapFrom(src => (DateTime?)null))
                .ForMember(dest => dest.Configured, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Removed, opt => opt.MapFrom(src => false));

            CreateMap<AgentUpdate.Command, Agent>()
                .ForPath(dest => dest.Id, option => option.MapFrom(src => src.Id));

            CreateMap<Agent, AgentResumeViewModel>()
                .ForMember(dest => dest.DisplayName, option => option.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.Company, option => option.MapFrom(src => src.Company.FantasyName))
                .ForMember(dest => dest.LastUpdate, option => option.MapFrom(src => src.UpdatedIn.ToString()))
                .ForMember(dest => dest.UserWhoCreated, option => option.MapFrom(src => $"{src.UserWhoCreatedName}"))
                .ForMember(dest => dest.Items, option => option.MapFrom(src => src.Items))
                .ForMember(dest => dest.CreatedIn, option => option.MapFrom(src => src.CreatedIn.ToString()));

            CreateMap<Agent, AgentDetailViewModel>()
                .ForMember(dest => dest.Id, option => option.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Configured, option => option.MapFrom(src => src.Configured))
                .ForMember(dest => dest.LastUpdate, option => option.MapFrom(src => src.UpdatedIn.ToString()))
                .ForMember(dest => dest.CreatedIn, option => option.MapFrom(src => src.CreatedIn.ToString()));

        }
    }
}
