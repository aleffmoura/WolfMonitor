using System;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.WpfApp.Applications.Users
{
    public class UserService : IUserService
    {
        private UserEndPoint _client;
        public UserService(UserEndPoint client)
        {
            _client = client;
        }

        public Result<Exception, UserBasicInformationViewModel> GetInfo()
        {
            return _client.GetInfo();
        }
    }
}
