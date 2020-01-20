using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users
{
    public class UserEndPoint : BaseEndPoint
    {
        public UserEndPoint(CustomHttpCliente customHttpCliente) : base(customHttpCliente)
        {

        }
        public Result<Exception, UserBasicInformationViewModel> GetInfo()
        {
            return InnerGetAsync<UserBasicInformationViewModel>("users/info").ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
