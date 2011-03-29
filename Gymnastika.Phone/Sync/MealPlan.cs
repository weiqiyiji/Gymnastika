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
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace Gymnastika.Phone.Sync
{
    [DataContract(Namespace="")]
    public class MealPlan
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public List<Meal> Meals { get;private set; }
        [DataMember]
        public DateTime Date { get; set; }
        public MealPlan()
        {
            Meals = new List<Meal>();
        }

    }
}
