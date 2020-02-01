using System.Windows.Controls;
using System.Windows.Media;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Monitorings.NovaPasta;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Services
{
    /// <summary>
    /// Interação lógica para ServiceUC.xam
    /// </summary>
    public partial class ServiceUC : UserControl
    {
        private SystemServiceViewModel _systemServiceViewModel;

        public ServiceUC(SystemServiceViewModel systemServiceViewModel)
        {
            InitializeComponent();
            _systemServiceViewModel = systemServiceViewModel;
            SetServiceValues();
        }

        public void SetServiceValues()
        {
            this.lblDisplayName.Text = _systemServiceViewModel.DisplayName;
            this.lblCurrentStatus.Text = _systemServiceViewModel.Value;
            this.lblLastStatus.Text = _systemServiceViewModel.LastStatus;
            this.lblServiceName.Text = _systemServiceViewModel.Name;

            ChangeColorTextBlock(this.lblCurrentStatus);

            ChangeColorTextBlock(this.lblLastStatus);
        }

        public void ChangeColorTextBlock(TextBlock textBlock)
        {
            if(textBlock.Text.Equals("running", System.StringComparison.InvariantCultureIgnoreCase))
            {
                textBlock.Foreground = new SolidColorBrush(Colors.Green);
            }
            else if(textBlock.Text.Equals("stopped", System.StringComparison.InvariantCultureIgnoreCase))
            {
                textBlock.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                textBlock.Foreground = new SolidColorBrush(Colors.Black);
            }
            OnApplyTemplate();
        }
    }
}
