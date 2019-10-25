using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Totten.Solutions.WolfMonitor.WpfApp.ViewModels.Agents;

namespace Totten.Solutions.WolfMonitor.WpfApp.Windows.DashBoards.UserControls.Agents
{
    /// <summary>
    /// Interação lógica para AgentsUserControl.xam
    /// </summary>
    public partial class AgentsUserControl : UserControl
    {
        public ObservableCollection<AgentViewModel> List { get; set; } = new ObservableCollection<AgentViewModel>();
        public AgentsUserControl()
        {
            List = new ObservableCollection<AgentViewModel>();
            InitializeComponent();
            var a = new AgentViewModel
            {
                Id = Guid.NewGuid(),
                Description = "Description in na description",
                Name = "name"
            };
            List.Add(a);
            List.Add(a);
            List.Add(a);
            List.Add(a);
            List.Add(a);
            List.Add(a);
            List.Add(a);
            List.Add(a);
            List.Add(a);
            List.Add(a);
            List.Add(a);
            List.Add(a);
            List.Add(a);
            List.Add(a);
            List.Add(a);
            List.Add(a);
            List.Add(a);
            List.Add(a);
            List.Add(a);
            //GridRoot.ItemsSource = List;
        }
    }
}
