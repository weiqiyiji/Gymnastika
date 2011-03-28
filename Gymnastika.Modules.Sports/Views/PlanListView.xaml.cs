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
using Gymnastika.Modules.Sports.Facilities;

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
            LoadViewModel();
        }

        void LoadViewModel()
        {
            AsychronousLoadHelper.AsychronousResolve<IPlanListViewModel>((model) =>
            {
                ViewModel = model;
            }, this.Dispatcher);
        }

        public IPlanListViewModel ViewModel
        {
            get { return DataContext as IPlanListViewModel; }
            set { DataContext = value; }
        }
    }
}
