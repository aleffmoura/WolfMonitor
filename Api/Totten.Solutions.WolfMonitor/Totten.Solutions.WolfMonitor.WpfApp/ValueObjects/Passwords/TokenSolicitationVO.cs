﻿using System;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Passwords
{
    public class TokenSolicitationVO
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public Guid RecoverSolicitationCode { get; set; }
    }
}
