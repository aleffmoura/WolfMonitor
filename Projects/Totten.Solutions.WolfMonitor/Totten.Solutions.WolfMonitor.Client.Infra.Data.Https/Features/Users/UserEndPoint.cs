using System;
using System.Net.Http;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users
{
    public class UserEndPoint : BaseEndPoint
    {
        private string _baseEndpoint = "users";

        public UserEndPoint(CustomHttpCliente customHttpCliente) : base(customHttpCliente) { }

        public async Task<Result<Exception, UserBasicInformationViewModel>> GetInfo()
            => await InnerGetAsync<UserBasicInformationViewModel>($"{_baseEndpoint}/info");

        public async Task<Result<Exception, PageResult<T>>> GetAll<T>()
            => await InnerGetAsync<PageResult<T>>(_baseEndpoint);

        public async Task<Result<Exception, PageResult<T>>> GetAllAgentsByUser<T>()
            => await InnerGetAsync<PageResult<T>>($"{_baseEndpoint}/agents");

        public async Task<Result<Exception, TReturn>> Post<TReturn, TPost>(string endpoint, TPost item)
            => await InnerAsync<TReturn, TPost>($"{_baseEndpoint}/{endpoint.TrimStart('/')}", item, HttpMethod.Post);
        
        public async Task<Result<Exception, TReturn>> Patch<TReturn, TPost>(string endpoint, TPost item)
            => await InnerAsync<TReturn, TPost>($"{_baseEndpoint}/{endpoint.TrimStart('/')}", item, HttpMethod.Patch);

        public async Task<Result<Exception, Guid>> Register<TPost>(string endpoint, TPost item)
            => await InnerAsync<Guid, TPost>($"{endpoint.TrimStart('/')}", item, HttpMethod.Post);

        public async Task<Result<Exception, Unit>> Delete(Guid userId)
            => await InnerAsync<Unit, object>($"{_baseEndpoint}/{userId}", null, HttpMethod.Delete);
    }
    

}
