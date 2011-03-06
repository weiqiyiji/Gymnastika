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

namespace Gymnastika.Modules.Meals.Widgets
{
    /// <summary>
    /// Interaction logic for BMIWidget.xaml
    /// </summary>
    [WidgetMetadata("BMI", "/Gymnastika.Modules.Meals;component/Images/BMI.jpg")]
    public partial class BMIWidget : IWidget
    {
        private readonly ISessionManager _sessionManager;
        private readonly IBMIIntroductionView _BMIIntroductionView;
        private readonly User _user;
        private readonly int _height;
        private readonly int _weight;
        private decimal BMI;

        private readonly Microsoft.Practices.Prism.Regions.IRegionManager _regionManager;
        private readonly Microsoft.Practices.Unity.IUnityContainer _container;

        public BMIWidget(IBMIIntroductionView BMIIntroductionView, ISessionManager sessionManager,
            Microsoft.Practices.Prism.Regions.IRegionManager regionManager,
            Microsoft.Practices.Unity.IUnityContainer container)
        {
            InitializeComponent();

            _BMIIntroductionView = BMIIntroductionView;
            _sessionManager = sessionManager;
            _regionManager = regionManager;
            _container = container;
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
            int NormalMetabolism = Int32.Parse(((_weight * 0.062 + 2.036) * 240).ToString());

            NormalMetabolismLabel.Text = NormalMetabolism.ToString();
        }

        private void BMIIntroduction_Click(object sender, RoutedEventArgs e)
        {
            _BMIIntroductionView.ShowView();
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Practices.Prism.Regions.IRegion displayRegion = _regionManager.Regions[Gymnastika.Common.RegionNames.DisplayRegion];

            Gymnastika.Modules.Meals.ViewModels.IMealsManagementViewModel mealsManagementViewModel = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<Gymnastika.Modules.Meals.ViewModels.IMealsManagementViewModel>();
            displayRegion.Add(mealsManagementViewModel.View);
            displayRegion.Activate(mealsManagementViewModel.View);
        }
    }
}
