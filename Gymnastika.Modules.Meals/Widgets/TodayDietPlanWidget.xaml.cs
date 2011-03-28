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
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Modules.Meals.Widgets
{
    /// <summary>
    /// Interaction logic for TodayDietPlanWidget.xaml
    /// </summary>
    [WidgetMetadata("饮食计划", "/Gymnastika.Modules.Meals;component/Images/DietPlan.png")]
    public partial class TodayDietPlanWidget : IWidget
    {
        public TodayDietPlanWidget(TodayDietPlanWidgetModel model)
        {
            InitializeComponent();

            Model = model;
        }

        public TodayDietPlanWidgetModel Model
        {
            get
            {
                return this.DataContext as TodayDietPlanWidgetModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        #region IWidget Members

        public void Initialize()
        {
            Model.Initialize();
        }

        #endregion
    }
}
