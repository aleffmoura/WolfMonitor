﻿using System;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Users.ViewModels
{
    public enum EUserLevel
    {
        User = 1,
        Admin = 2,
        System = 3
    }
    public class UserBasicInformationViewModel
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string FullName { get; set; }
        public int UserLevel { get; set; }
        public static string Token { get; set; }

        public Guid UserId { get; set; }
    }
}
