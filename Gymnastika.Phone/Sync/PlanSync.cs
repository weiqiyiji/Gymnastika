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
using System.Threading;
using Gymnastika.Phone.Common;

namespace Gymnastika.Phone.Sync
{
    public class PlanSync
    {
        public MealPlan GetMealPlan(string userId)
        {
            AutoResetEvent wait = new AutoResetEvent(false);
            HttpClient client = new HttpClient();
            HttpClient.GetCompeletedArgs arg;
            client.GetCompeleted+=new EventHandler<HttpClient.GetCompeletedArgs>((s,e)=>
            {
                arg = e;
                ((EventWaitHandle)e.UserToken).Set();
            });
            client.Get(Config.GetServerPathUri(
                string.Format(Config.GetPlanOfTodayServiceUri, HttpUtility.UrlEncode(userId))), wait);
            wait.WaitOne();
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
        public delegate void CompeleteTaskCallback(int id,bool successful);
        public void CompeleteTask(int Id,CompeleteTaskCallback callback)
        {
            HttpClient client = new HttpClient();
            client.GetCompeleted+=new EventHandler<HttpClient.GetCompeletedArgs>((s,e)=>
            {
                if (callback != null)
                {
                    callback(Id, e.Error == null);
                }
            });
            client.Get(Config.GetServerPathUri(string.Format(Config.CompeleteTaskUri, Id)), null);
        }

        public void DelayMeal(Meal meal, TimeSpan span)
        {

        }
        public void DelaySport(Sport sport, TimeSpan span)
        {

        }
    }
}