using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using Gymnastika.Modules.Meals.ViewModels;

namespace Gymnastika.Modules.Meals.Views
{
    /// <summary>
    /// Interaction logic for AmountSelectorView.xaml
    /// </summary>
    public partial class AmountSelectorView : UserControl
    {
        public AmountSelectorView()
        {
            InitializeComponent();
        }

        [Dependency]
        public AmountSelectorViewModel Model
        {
            get { return this.DataContext as AmountSelectorViewModel; }
            set { this.DataContext = value; }
        }

        private void UserControl_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
        }

        private void UserControl_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            Model.CurrentValue = (int)e.DeltaManipulation.Translation.Y;
        }
    }
}
