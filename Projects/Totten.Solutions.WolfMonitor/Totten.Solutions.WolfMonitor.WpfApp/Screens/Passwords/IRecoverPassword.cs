using System;
using System.Threading.Tasks;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Passwords
{
    public interface IRecoverPassword
    {
        Task<object> Validation(object param);
        EventHandler OnChangeEvent { get; set; }
        StepRecover StepRecover { get; }
        bool EnablePrev { get; }
        bool EnableNext { get; }
        string BtnNextName { get; }

    }
}
