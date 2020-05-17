using AutoMapper;
using Totten.Solutions.WolfMonitor.Application.Features.Companies.Handlers;
using Totten.Solutions.WolfMonitor.Application.Features.Companies.ViewModels;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;

namespace Totten.Solutions.WolfMonitor.Application.Features.Companies
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CompanyCreate.Command, Company>()
                .ForMember(dest => dest.Removed, opt => opt.MapFrom(src => false));

            CreateMap<Company, CompanyResumeViewModel>();

            CreateMap<Company, CompanyDetailViewModel>();

            CreateMap<CompanyResumeViewModel, CompanyResumeViewModel>();
        }
    }
}
