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

namespace Gymnastika.Modules.Sports.Views
{
    /// <summary>
    /// Interaction logic for PlanListView.xaml
    /// </summary>
    public partial class PlanListView : UserControl, IPlanListView
    {

        public PlanListView()
        {
            InitializeComponent();
            SetViewModel();
        }

        private void SetViewModel()
        {
            try
            {
                IServiceLocator servicelocator = ServiceLocator.Current;
                if (servicelocator != null)
                    ViewModel = servicelocator.GetInstance<IPlanListViewModel>();
            }catch(Exception)
            {

            }
        }

        public IPlanListViewModel ViewModel
        {
            get { return DataContext as IPlanListViewModel; }
            set { DataContext = value; }
        }



        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            private set { SetValue(IsExpandedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsExpanded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(PlanListView), new PropertyMetadata(false));
        
        public void Expand()
        {
            if (IsExpanded == false)
            {
                IsExpanded = true;
                (FindResource("FlyOutStoryboard") as Storyboard).Begin();
                //LastWeek.BeginAnimation(LastWeek.RenderTransform
            }
        }

        public void Minimize()
        {
            if (IsExpanded == true)
            {
                IsExpanded = false;
                (FindResource("FlyInStoryboard") as Storyboard).Begin();
            }
        }

    }

    public interface IPlanListView
    {

    }
}
