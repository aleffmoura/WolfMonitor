using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Helpers.Validators;

namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.Helpers
{
    public class ValidatorHelper
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                if (mailAddress.Host.Contains("."))
                {
                    Ping pingSender = new Ping();
                    var result = pingSender.Send(mailAddress.Host, timeout: 12000, buffer: Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"), new PingOptions(52, true));
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidCPF(string cpf)
            => ValidateCpf.IsCpf(cpf);

        public static bool IsValidCNPJ(string cpf)
            => ValidateCpf.IsCpf(cpf);

    }
}
