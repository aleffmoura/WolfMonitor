using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Application.Features.Companies.ViewModels
{
    public class CompanyDetailViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Cnpj { get; set; }
        public string FantasyName { get; set; }
        public string StateRegistration { get; set; }
        public string MunicipalRegistration { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
