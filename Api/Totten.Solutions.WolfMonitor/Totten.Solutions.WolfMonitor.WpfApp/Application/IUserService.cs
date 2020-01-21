using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.WpfApp.Applications
{
    public interface IUserService
    {
        Task<Result<Exception, UserBasicInformationViewModel>> GetInfo();
    }
}
