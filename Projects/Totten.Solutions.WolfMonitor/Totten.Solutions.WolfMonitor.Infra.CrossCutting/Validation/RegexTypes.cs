namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.Validation
{
    public static class RegexTypes
    {

        public const string MacAddress = @"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$";

        public const string IpAddress = @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";

    }
}
