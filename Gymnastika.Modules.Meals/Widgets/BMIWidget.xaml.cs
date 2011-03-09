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
using Gymnastika.Widgets;
using Gymnastika.Services.Session;
using Gymnastika.Services.Models;
using Gymnastika.Modules.Meals.Views;
using Microsoft.Practices.Prism.Regions;
using Gymnastika.Modules.Meals.ViewModels;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Modules.Meals.Controllers;
using Microsoft.Practices.Unity;

namespace Gymnastika.Modules.Meals.Widgets
{
    /// <summary>
    /// Interaction logic for BMIWidget.xaml
    /// </summary>
    [WidgetMetadata("BMI", "/Gymnastika.Modules.Meals;component/Images/BMI.jpg")]
    public partial class BMIWidget : IWidget
    {
        private readonly ISessionManager _sessionManager;
        private readonly User _user;
        private readonly int _height;
        private readonly int _weight;
        private decimal BMI;

        private readonly IRegionManager _regionManager;
        private readonly ILoadDataController _loadDataController;

        public BMIWidget(ISessionManager sessionManager,
            IRegionManager regionManager
            //,ILoadDataController loadDataController
            )
        {
            InitializeComponent();

            //_loadDataController = loadDataController;

            _sessionManager = sessionManager;
            _regionManager = regionManager;
            _user = _sessionManager.GetCurrentSession().AssociatedUser;
            _height = _user.Height;
            _weight = _user.Weight;
        }

        #region IWidget Members

        public void Initialize()
        {
            InitializeBMI();
            InitializeSuggestion();
            InitializeNormalWeight();
            InitializeBeatyWeight();
            InitializeNormalMetabolism();
        }

        #endregion

        private void InitializeBMI()
        {
            BMI = _weight / (_height * _height / 10000);

            BMILabel.Text = "" + Decimal.Round(BMI, 1).ToString();
        }

        private void InitializeSuggestion()
        {
            if (BMI < 15) { SuggestionLabel.Text = "您太瘦了哦，应去做个体检，增加营养。"; }

            else if (BMI >= 15 && BMI < 18) { SuggestionLabel.Text = "您过度苗条，应增加营养和锻炼。"; }

            else if (BMI >= 18 && BMI < 22) { SuggestionLabel.Text = "恭喜！！您是标准身材,注意保持。"; }

            else if (BMI >= 22 && BMI < 25) { SuggestionLabel.Text = "您是健康体重,但已不苗条，小心哦~"; }

            else if (BMI >= 25 && BMI < 30) { SuggestionLabel.Text = "您超重了，应该立即减肥！"; }

            else if (BMI >= 30 && BMI < 40) { SuggestionLabel.Text = "您太胖了哦，减肥已是您的头等大事！"; }

            else { SuggestionLabel.Text = "您非常胖，肥胖将危及您的健康！"; }
        }

        private void InitializeNormalWeight()
        {
            int NormalWeight = (_height * _height / 10000) * 22;

            NormalWeightLabel.Text = NormalWeight.ToString();
        }

        private void InitializeBeatyWeight()
        {
            int BeautyWeight = (_height * _height / 10000) * 19;

            BeautyWeightLabel.Text = BeautyWeight.ToString();
        }

        private void InitializeNormalMetabolism()
        {
            double NormalMetabolism = (_weight * 0.062 + 2.036) * 240;

            NormalMetabolismLabel.Text = NormalMetabolism.ToString();
        }

        private void BMIIntroduction_Click(object sender, RoutedEventArgs e)
        {
            IBMIIntroductionView BMIIntroductionView = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IBMIIntroductionView>();
            BMIIntroductionView.ShowView();
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            IRegion displayRegion = _regionManager.Regions[Gymnastika.Common.RegionNames.DisplayRegion];

            IMealsManagementViewModel mealsManagementViewModel = ServiceLocator.Current.GetInstance<IMealsManagementViewModel>();
            displayRegion.Add(mealsManagementViewModel.View);
            displayRegion.Activate(mealsManagementViewModel.View);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _loadDataController.LoadCategoryData();
            MessageBox.Show("已保存");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _loadDataController.LoadSubCategoryData();
            MessageBox.Show("已保存");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            _loadDataController.LoadFoodData();
            MessageBox.Show("已保存");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            _loadDataController.LoadNutritionalElementData();
            MessageBox.Show("已保存");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            _loadDataController.LoadIntroductionData();
            MessageBox.Show("已保存");
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            _loadDataController.LoadDietPlanData();
            MessageBox.Show("已保存");
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            _loadDataController.LoadSubDietPlanData();
            MessageBox.Show("已保存");
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            _loadDataController.LoadDietPlanItemData();
            MessageBox.Show("已保存");
        }
    }
}
