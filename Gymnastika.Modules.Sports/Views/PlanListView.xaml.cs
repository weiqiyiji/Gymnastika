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
            set 
            {
                DataContext = value;
                value.SelectedItemChangedEvent += OnSelectedItemChange;
            }
        }

        void OnSelectedItemChange(object d, EventArgs args)
        {
            UpdateState();
        }

        void UpdateState()
        {
            if (planDetailView != null && ViewModel != null && planDetailView.ViewModel != ViewModel.SelectedItem)
                planDetailView.ViewModel = ViewModel.SelectedItem;
        }

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            private set { SetValue(IsExpandedProperty, value); }
        }

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(PlanListView), new PropertyMetadata(false));
        
        public void Expand()
        {
            if (IsExpanded == false)
            {
                IsExpanded = true;
                (FindResource("FlyOutStoryboard") as Storyboard).Begin();
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
        void Expand();
        void Minimize();
    }
}
