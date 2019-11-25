namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs
{
    public partial class Option
    {
        public static Option<T> Of<T>(T value) => new Option<T>(value, value != null);
    }
}
