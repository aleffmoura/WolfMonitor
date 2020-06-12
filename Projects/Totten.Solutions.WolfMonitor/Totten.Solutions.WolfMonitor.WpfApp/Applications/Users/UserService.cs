using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Companies;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Passwords;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Users;

namespace Totten.Solutions.WolfMonitor.WpfApp.Applications.Users
{
    public class UserService : IUserService
    {
        private UserEndPoint _endPoint;
        private CompanyEndPoint _companyEndPoint;

        public UserService(UserEndPoint endPoint, CompanyEndPoint companyEndPoint)
        {
            _endPoint = endPoint;
            _companyEndPoint = companyEndPoint;
        }

        public Task<Result<Exception, Guid>> Post(UserCreateVO user, Guid companyId)
            => _endPoint.Register($"register/users/{companyId}", user);

        public Task<Result<Exception, Unit>> UpdateMyInfo(UserUpdateVO user)
            => _endPoint.Patch<Unit, UserUpdateVO>($"info", user);

        public async Task<Result<Exception, UserBasicInformationViewModel>> GetInfo()
            => await _endPoint.GetInfo();

        public async Task<Result<Exception, Unit>> Delete(Guid agentId)
            => await _endPoint.Delete(agentId);

        public Task<Result<Exception, PageResult<UserResumeViewModel>>> GetAll()
            => _endPoint.GetAll<UserResumeViewModel>();

        public Task<Result<Exception, PageResult<UserResumeViewModel>>> GetAll(Guid companyId)
            => _companyEndPoint.GetAllUsers<UserResumeViewModel>(companyId);

        public Task<Result<Exception, PageResult<AgentForUserViewModel>>> GetAllAgentsByUser()
            => _endPoint.GetAllAgentsByUser<AgentForUserViewModel>();

        public string GetClientCredentials()
            => Convert.ToBase64String(Encoding.ASCII.GetBytes($"application:applicationSecret"));

        public Task<Result<Exception, Guid>> RecoverPassword(string company, string login, string email)
            => _endPoint.Post<Guid, RecoverSolicitationRequestVO>("forgotPassword", new RecoverSolicitationRequestVO { Company = company, Login = login, Email = email });

        public Task<Result<Exception, Guid>> ReSendToken(string company, string login, string email)
            => _endPoint.Post<Guid, RecoverSolicitationRequestVO>("forgotPassword/token-resend", new RecoverSolicitationRequestVO { Company = company, Login = login, Email = email });

        public Task<Result<Exception, Guid>> TokenConfimation(string company, string login, string email, Guid recoverSolicitationCode, Guid token)
            => _endPoint.Post<Guid, TokenConfirmationRequestVO>("forgotPassword/validate-token", new TokenConfirmationRequestVO
            {
                Company = company,
                Login = login,
                Email = email,
                RecoverSolicitationCode = recoverSolicitationCode,
                Token = token
            });

        public Task<Result<Exception, Unit>> ChangePassword(string company, string login, string email, Guid tokenSolicitationCode, Guid recoverSolicitationCode, string password)
         => _endPoint.Post<Unit, TokenChangePasswordVO>("forgotPassword/change-password", new TokenChangePasswordVO
         {
             Company = company,
             Login = login,
             Email = email,
             TokenSolicitationCode = tokenSolicitationCode,
             RecoverSolicitationCode = recoverSolicitationCode,
             Password = password
         });

        public async Task<string> Authentication()
        {
            var request = _endPoint.Client.CreateRequest(HttpMethod.Post, "identityserver/connect/token");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", GetClientCredentials());
            request.Headers.Host = _endPoint.Client.UrlApi.Replace("http://", "");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "password"},
                { "username",  _endPoint.Client.User.Login },
                { "password",  _endPoint.Client.User.Password},
                { "scope", "Agents Monitoring Users Companies"}
            });

            using (var httpClient = new HttpClient())
            using (request)
            using (var response = await httpClient.SendAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Erro na comunicação com a API, não foi possivel obter o token. status de erro: {response.StatusCode}");
                }

                dynamic content = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult());
                return content.access_token.ToString();
            }
        }


    }
}
