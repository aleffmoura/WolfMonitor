using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Requests;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users
{
    public class UserEndPoint : BaseEndPoint
    {
        public UserEndPoint(CustomHttpCliente customHttpCliente) : base(customHttpCliente)
        {

        }
        public async Task<Result<Exception, UserBasicInformationViewModel>> GetInfo()
        {
            return await InnerGetAsync<UserBasicInformationViewModel>("users/info");
        }

    }
}
