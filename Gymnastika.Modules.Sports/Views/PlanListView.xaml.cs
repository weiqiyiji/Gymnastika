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
using Gymnastika.Modules.Sports.ViewModels;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Common.Navigation;
using System.Windows.Media.Animation;
using Microsoft.Surface.Presentation.Controls;

namespace Gymnastika.Modules.Sports.Views
{
    /// <summary>
    /// Interaction logic for PlanListView.xaml
    /// </summary>
    public partial class PlanListView : UserControl
    {
        public PlanListView()
        {
            InitializeComponent();
        }

        public IPlanListViewModel ViewModel
        {
            get { return DataContext as IPlanListViewModel; }
            set { DataContext = value; }
        }
    }
    //public class SurfaceListBoxWithoutHighlight : SurfaceListBox
    //{
    //    protected override DependencyObject GetContainerForItemOverride()
    //    {
    //        return base.GetContainerForItemOverride() as Grid);
    //    }
    //}
}
