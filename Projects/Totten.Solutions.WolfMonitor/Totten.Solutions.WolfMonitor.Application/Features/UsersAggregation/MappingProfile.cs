using AutoMapper;
using System;
using Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers;
using Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.ViewModels;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

namespace Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserCreate.Command, User>()
                .ForMember(dest => dest.CreatedIn, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedIn, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Removed, opt => opt.MapFrom(src => false));

            CreateMap<UserUpdate.Command, User>()
                .ForMember(dest => dest.UpdatedIn, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<User, UserResumeViewModel>()
                .ForMember(dest => dest.UserLevel, opt => opt.MapFrom(src => (int)src.Role.Level))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
                .ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => src.LastLogin.HasValue ? src.LastLogin.Value.ToString("dd/MM/yyyy hh:mm") : "Nunca conectado"))
                .ForMember(dest => dest.FullName, option => option.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<User, UserDetailViewModel>()
                .ForMember(dest => dest.UserLevel, opt => opt.MapFrom(src => (int)src.Role.Level))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
                .ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => src.LastLogin.HasValue ? src.LastLogin.Value.ToString("dd/MM/yyyy hh:mm") : "Nunca conectado"))
                .ForMember(dest => dest.FullName, option => option.MapFrom(src => $"{src.FirstName} {src.LastName}"));



            
            CreateMap<UserResumeViewModel, User>();
        }
    }
}
