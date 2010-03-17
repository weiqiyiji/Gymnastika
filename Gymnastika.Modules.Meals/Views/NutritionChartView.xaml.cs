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
        private DoubleAnimation _widthAnimation;

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

        public void DietPlanCalorieValueChangeAnimation(double oldValue, double newValue)
        {
            NutritionValueChangeAnimation(oldValue, newValue);
            //Storyboard.SetTargetName(_widthAnimation, "DietPlanCalorieValueBorder");
            //Storyboard.SetTargetProperty(_widthAnimation, new PropertyPath(Border.WidthProperty));
            //Storyboard storyboard = new Storyboard();
            //storyboard.Children.Add(_widthAnimation);
            //storyboard.Begin(DietPlanCalorieValueBorder);
            //DietPlanCalorieValueBorder.BeginAnimation(WidthProperty, _widthAnimation);
            DietPlanCalorieValueBorder.Width = 100;
        }

        public void DietPlanCalorieValueChangeAnimation(AnimationTimeline animation)
        {
            //DietPlanCalorieValueBorder.BeginAnimation(WidthProperty, animation);
            //DietPlanCalorieValueBorder.Width = 100;
        }

        #endregion

        private void NutritionValueChangeAnimation(double oldValue, double newValue)
        {
            _widthAnimation = new DoubleAnimation();
            _widthAnimation.From = oldValue;
            _widthAnimation.To = newValue;
            _widthAnimation.Duration = TimeSpan.FromSeconds(2);
        }
    }
}
