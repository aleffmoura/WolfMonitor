using System;
using System.Net.Http;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Companies
{
    public class CompanyEndPoint : BaseEndPoint
    {
        private string _baseEndpoint = "companies";

        public CompanyEndPoint(CustomHttpCliente customHttpCliente) : base(customHttpCliente)
        {

        }

        public async Task<Result<Exception, TR>> Send<TR, T>(string endpoint, T obj, HttpMethod httpMethod)
            => await InnerAsync<TR, T>($"{_baseEndpoint}/{endpoint.TrimStart('/')}", obj, httpMethod);

        public async Task<Result<Exception, Guid>> Post<T>(T company)
            => await InnerAsync<Guid, T>(_baseEndpoint, company, HttpMethod.Post);

        public async Task<Result<Exception, T>> GetDetail<T>(Guid id)
            => await InnerGetAsync<T>($"{_baseEndpoint}/{id}");

        public async Task<Result<Exception, T>> GetResume<T>(Guid companyId)
            => await InnerGetAsync<T>($"{_baseEndpoint}/{companyId}");

        public Task<Result<Exception, PageResult<T>>> GetAll<T>()
            => InnerGetAsync<PageResult<T>>(_baseEndpoint);

        public Task<Result<Exception, PageResult<T>>> GetAllUsers<T>(Guid companyId)
            => InnerGetAsync<PageResult<T>>($"{_baseEndpoint}/{companyId}/users");

        public async Task<Result<Exception, Unit>> Delete(Guid companyId)
            => await InnerAsync<Unit, object>($"{_baseEndpoint}/{companyId}", null, HttpMethod.Delete);

        public bool Update<T>(T company)
            => InnerAsync<T>(company, HttpMethod.Patch).ConfigureAwait(false).GetAwaiter().GetResult().IsSuccess;

        private async Task<Result<Exception, Unit>> InnerAsync<T>(T company, HttpMethod httpMethod)
            => await InnerAsync<Unit, T>(_baseEndpoint, company, httpMethod);

    }
}
