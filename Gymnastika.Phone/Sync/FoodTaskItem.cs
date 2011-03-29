using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gymnastika.Phone.Sync
{
    [DataContract]
    public class FoodTaskItem
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string FoodName { get; set; }

        [DataMember]
        public int Calorie { get; set; }

        [DataMember]
        public int Amount { get; set; }
    }

    [CollectionDataContract]
    public class FoodTaskList : List<FoodTaskItem>
    {
    }
}
