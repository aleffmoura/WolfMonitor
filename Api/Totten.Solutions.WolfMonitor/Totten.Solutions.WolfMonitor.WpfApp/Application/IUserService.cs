using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Users.ViewModels;

namespace Totten.Solutions.WolfMonitor.WpfApp.Applications
{
    public interface IUserService
    {
        UserLoginViewModel Authentication(string userName, string password);
    }
}
