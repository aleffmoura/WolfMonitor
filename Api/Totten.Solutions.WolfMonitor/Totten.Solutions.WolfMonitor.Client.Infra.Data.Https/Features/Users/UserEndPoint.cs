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
        private string _primaryEndpoint = "users";

        public UserEndPoint(CustomHttpCliente customHttpCliente) : base(customHttpCliente) { }

        public async Task<Result<Exception, UserBasicInformationViewModel>> GetInfo()
            => await InnerGetAsync<UserBasicInformationViewModel>($"{_primaryEndpoint}/info");

        public async Task<Result<Exception, TReturn>> Post<TReturn, TPost>(string endpoint, TPost item)
            => await InnerAsync<TReturn, TPost>($"{_primaryEndpoint}/{endpoint.TrimStart('/')}", item, HttpMethod.Post);
    }
    

}
