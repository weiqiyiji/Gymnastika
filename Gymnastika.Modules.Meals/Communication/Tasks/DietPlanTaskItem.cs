using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gymnastika.Modules.Meals.Communication.Tasks
{
    [DataContract(Namespace="")]
    public class DietPlanTaskItem
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime StartTime {get;set;}

        [DataMember]
        public int Score { get; set; }

        [DataMember]
        public List<FoodTaskItem> FoodTasks { get; set; }
    }
}
