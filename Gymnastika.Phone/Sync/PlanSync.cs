using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gymnastika.Phone.Sync
{
    public class PlanSync
    {
        public MealPlan GetMealPlan(string xml)
        {
            return null;
        }
        public SportPlan GetSportPlan(string xml)
        {
            return null;
        }
        public MealPlan GetMealPlan()
        {
            return null;
        }
        public SportPlan GetSportPlan()
        {
            return null;
        }
        public void UpdateSportStatus(Sport sport, Common.ScheduleItemStatus status)
        {

        }
        public void UpdateSportPlanStatus(SportPlan plan, Common.ScheduleItemStatus status)
        {

        }
        public void UpdateMealStatus(Meal meal, Common.ScheduleItemStatus status)
        {

        }
        public void UpdateMealPlanStatus(MealPlan plan, Common.ScheduleItemStatus status)
        {

        }
        public void DelayMeal(Meal meal, TimeSpan span)
        {

        }
        public void DelaySport(Sport sport, TimeSpan span)
        {

        }
    }
}