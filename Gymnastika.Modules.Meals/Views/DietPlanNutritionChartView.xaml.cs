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
    /// Interaction logic for DietPlanNutritionChartView.xaml
    /// </summary>
    public partial class DietPlanNutritionChartView : IDietPlanNutritionChartView
    {
        public DietPlanNutritionChartView()
        {
            InitializeComponent();
        }

        #region IDietPlanNutritionChartView Members

        public IDietPlanNutritionChartViewModel Context
        {
            get
            {
                return this.DataContext as IDietPlanNutritionChartViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        #endregion
    }
}
