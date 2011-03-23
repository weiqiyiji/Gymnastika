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
using System.Windows.Media.Animation;

namespace Gymnastika.Modules.Meals.Views
{
    /// <summary>
    /// Interaction logic for NutritionChartView.xaml
    /// </summary>
    public partial class NutritionChartView : INutritionChartView
    {
        public NutritionChartView()
        {
            InitializeComponent();
        }

        #region INutritionChartView Members

        public INutritionChartViewModel Context
        {
            get
            {
                return this.DataContext as INutritionChartViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        #endregion
    }
}
