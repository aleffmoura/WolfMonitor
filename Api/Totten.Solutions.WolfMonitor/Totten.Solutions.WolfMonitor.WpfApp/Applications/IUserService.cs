using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Users;

namespace Totten.Solutions.WolfMonitor.WpfApp.Applications
{
    public interface IUserService
    {
        Task<Result<Exception, Guid>> Post(UserCreateVO user);
        Task<Result<Exception, PageResult<UserResumeViewModel>>> GetAll();
        Task<Result<Exception, UserBasicInformationViewModel>> GetInfo();
        Task<Result<Exception, Unit>> Delete(Guid userId);
        Task<Result<Exception, Guid>> RecoverPassword(string login, string email);
        Task<Result<Exception, Guid>> TokenConfimation(string login, string email, Guid recoverSolicitationCode, Guid token);
        Task<Result<Exception, Unit>> ChangePassword(string login, string email, Guid tokenSolicitationCode, Guid RecoverSolicitationCode, string pass);
    }
}
