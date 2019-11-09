using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Totten.Solutions.WolfMonitor.Authorize
{

    public class AppClient
    {
        [Key]
        [MaxLength(32)]
        public string ClientId { get; set; }

        [MaxLength(80)]
        [Required]
        public string Base64Secret { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}
