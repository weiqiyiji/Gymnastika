using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;

namespace Gymnastika.Modules.Meals.Communication.Tasks
{
    public class DietPlanTaskItem
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime StartTime {get;set;}

        [DataMember]
        public int Score { get; set; }

        [DataMember]
        public FoodTaskList FoodTasks { get; set; }
        //[DataMember]
        //public FoodTaskItem[] FoodTasks { get; set; }
    }
}
