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
using Gymnastika.Common;

namespace Gymnastika.Modules.Meals.Widgets
{
    /// <summary>
    /// Interaction logic for BMIWidget.xaml
    /// </summary>
    [WidgetMetadata("BMI", "/Gymnastika.Modules.Meals;component/Images/BMI.png")]
    public partial class BMIWidget : IWidget
    {
        private readonly ISessionManager _sessionManager;
        private readonly IUnityContainer _container;
        private readonly User _user;
        private readonly int _height;
        private readonly int _weight;

        public BMIWidget(ISessionManager sessionManager,
            IRegionManager regionManager,
            IUnityContainer container)
        {
            InitializeComponent();


            _container = container;
            _sessionManager = sessionManager;
            _user = _sessionManager.GetCurrentSession().AssociatedUser;
            _height = _user.Height;
            _weight = _user.Weight;

        }

        //public int MinBMIValue { get; set; }

        //public int MaxBMIValue { get; set; }

        public int BMIValue { get; set; }

        #region IWidget Members

        public void Initialize()
        {
            InitializeBMI();
            //InitializeSuggestion();
            InitializeNormalWeight();
            InitializeBeatyWeight();
            InitializeNormalMetabolism();
        }

        #endregion

        private void InitializeBMI()
        {
            BMIValue = (int)(_weight / ((double)_height * _height / 10000));

            BMILabel.Text = BMIValue.ToString();
        }

        //private void InitializeSuggestion()
        //{
        //    if (BMI < 15) { SuggestionLabel.Text = "您太瘦了哦，应去做个体检，增加营养。"; }

        //    else if (BMI >= 15 && BMI < 18) { SuggestionLabel.Text = "您过度苗条，应增加营养和锻炼。"; }

        //    else if (BMI >= 18 && BMI < 22) { SuggestionLabel.Text = "恭喜！！您是标准身材,注意保持。"; }

        //    else if (BMI >= 22 && BMI < 25) { SuggestionLabel.Text = "您是健康体重,但已不苗条，小心哦~"; }

        //    else if (BMI >= 25 && BMI < 30) { SuggestionLabel.Text = "您超重了，应该立即减肥！"; }

        //    else if (BMI >= 30 && BMI < 40) { SuggestionLabel.Text = "您太胖了哦，减肥已是您的头等大事！"; }

        //    else { SuggestionLabel.Text = "您非常胖，肥胖将危及您的健康！"; }
        //}

        private void InitializeNormalWeight()
        {
            int NormalWeight = (int)(((double)_height * _height / 10000) * 22);

            NormalWeightLabel.Text = NormalWeight.ToString();
        }

        private void InitializeBeatyWeight()
        {
            int BeautyWeight = (int)(((double)_height * (double)_height / 10000d) * 19d);

            BeautyWeightLabel.Text = BeautyWeight.ToString();
        }

        private void InitializeNormalMetabolism()
        {
            double NormalMetabolism = (_weight * 0.062 + 2.036) * 240;

            NormalMetabolismLabel.Text = NormalMetabolism.ToString();
        }

        private void BMIIntroduction_Click(object sender, RoutedEventArgs e)
        {
            IBMIIntroductionView BMIIntroductionView = _container.Resolve<IBMIIntroductionView>();
            BMIIntroductionView.ShowView();
        }
    }
}
