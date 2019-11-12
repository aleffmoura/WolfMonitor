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

            CreateMap<User, UserResumeViewModel>()
                .ForMember(dest => dest.FullName, option => option.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }
    }
}
