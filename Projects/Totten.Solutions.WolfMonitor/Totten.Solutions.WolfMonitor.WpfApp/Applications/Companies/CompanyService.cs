using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Companies;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Companies;

namespace Totten.Solutions.WolfMonitor.WpfApp.Applications.Companies
{
    public class CompanyService
    {
        private CompanyEndPoint _endPoint;

        public CompanyService(CompanyEndPoint endPoint)
        {
            _endPoint = endPoint;
        }


        public Task<Result<Exception, PageResult<CompanyResumeViewModel>>> GetAll()
            => _endPoint.GetAll<CompanyResumeViewModel>();

        public async Task<Result<Exception, CompanyResumeViewModel>> GetResume(Guid companyId)
            => await _endPoint.GetResume<CompanyResumeViewModel>(companyId);

        public async Task<Result<Exception, CompanyDetailViewModel>> GetDetail(Guid companyId)
            => await _endPoint.GetResume<CompanyDetailViewModel>(companyId);

        public async Task<Result<Exception, Unit>> Delete(Guid companyId)
            => await _endPoint.Delete(companyId);

        public async Task<Result<Exception, Guid>> Post(object company)
            => await _endPoint.Post(company);
    }
}