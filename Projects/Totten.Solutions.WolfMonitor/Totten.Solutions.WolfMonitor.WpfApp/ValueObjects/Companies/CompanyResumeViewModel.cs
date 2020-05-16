using System;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Companies
{
    public class CompanyResumeViewModel
    {
        public Guid Id { get; set; }
        public string Company { get; set; }
        public int QtdAgents { get; set; }
        public int QtdServices { get; set; }
        public int QtdArchives { get; set; }
        public int QtdUsers { get; set; }
    }
}
