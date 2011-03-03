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
    /// Interaction logic for DietPlanSubListView.xaml
    /// </summary>
    public partial class DietPlanSubListView : IDietPlanSubListView
    {
        public DietPlanSubListView()
        {
            InitializeComponent();
        }

        #region IDietPlanSubListView Members

        public IDietPlanSubListViewModel Context
        {
            get
            {
                return this.DataContext as IDietPlanSubListViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        public void Expand()
        {
            SubDietPlanList.ClearValue(Control.HeightProperty);
        }

        #endregion
    }
}
