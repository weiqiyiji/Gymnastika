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
using Gymnastika.Modules.Meals.ViewModels;

namespace Gymnastika.Modules.Meals.Views
{
    /// <summary>
    /// Interaction logic for DietPlanListView.xaml
    /// </summary>
    public partial class DietPlanListView : IDietPlanListView
    {
        public DietPlanListView()
        {
            InitializeComponent();
        }

        #region IDietPlanView Members

        public IDietPlanListViewModel Context
        {
            get
            {
                return this.DataContext as IDietPlanListViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        #endregion

        private void TraverseChildItemsControls(DependencyObject obj)
        {
            DependencyObject child = null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is ItemsControl)
                {
                    child.ClearValue(HeightProperty);
                }
                else
                {
                    TraverseChildItemsControls(child);
                }
            }
        }
    }
}
