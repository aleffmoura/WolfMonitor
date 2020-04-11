using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.WpfApp.Applications
{
    public interface IUserService
    {
        Task<Result<Exception, UserBasicInformationViewModel>> GetInfo();
        Task<Result<Exception, string>> RecoverPassword(string username, string email);
        Task<Result<Exception, string>> TokenConfimation(string username, string email, string recoverSolicitationCode, string token);
        Task<Result<Exception, Unit>> ChangePassword(string username, string email, string tokenSolicitationCode, string pass, string repass);
    }
}
