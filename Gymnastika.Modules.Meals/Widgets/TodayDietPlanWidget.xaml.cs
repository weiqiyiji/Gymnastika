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
using Gymnastika.Modules.Meals.ViewModels;
using Gymnastika.Modules.Meals.Services;
using Gymnastika.Data;
using Gymnastika.Modules.Meals.Models;
using Gymnastika.Services.Session;
using Gymnastika.Services.Models;

namespace Gymnastika.Modules.Meals.Widgets
{
    /// <summary>
    /// Interaction logic for TodayDietPlanWidget.xaml
    /// </summary>
    [WidgetMetadata("饮食计划", "/Gymnastika.Modules.Meals;component/Images/DietPlan.png")]
    public partial class TodayDietPlanWidget : IWidget
    {
        private readonly IFoodService _foodService;
        private readonly IWorkEnvironment _workEnvironment;
        private readonly ISessionManager _sessionManager;
        private readonly User _currentUser;

        public TodayDietPlanWidget(IFoodService foodService, 
            IWorkEnvironment workEnvironment,
            ISessionManager sessionManager)
        {
            _foodService = foodService;
            _workEnvironment = workEnvironment;
            _sessionManager = sessionManager;
            _currentUser = _sessionManager.GetCurrentSession().AssociatedUser;

            using (var scope = _workEnvironment.GetWorkContextScope())
            {
                DietPlan = _foodService.DietPlanProvider.Get(_currentUser, DateTime.Today);
                DietPlan.SubDietPlans = DietPlan.SubDietPlans.ToList();
                foreach (var subDietPlan in DietPlan.SubDietPlans)
                {
                    subDietPlan.DietPlanItems = subDietPlan.DietPlanItems.ToList();
                    foreach (var dietPlanItem in subDietPlan.DietPlanItems)
                    {
                        dietPlanItem.Food = _foodService.FoodProvider.Get(dietPlanItem);
                    }
                }
            }

            InitializeComponent();
        }

        public DietPlan DietPlan { get; set; }

        public DietPlanItemViewModel TodayDietPlanViewModel { get; set; }

        #region IWidget Members

        public void Initialize()
        {
            if (DietPlan != null)
                TodayDietPlanViewModel = new DietPlanItemViewModel(DietPlan);
        }

        #endregion
    }
}
