using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gymnastika.Phone.Sync
{
    [DataContract(Namespace = "")]
    public class SportsPlanTaskItem
    {
        [DataMember]
        public  int Id { get; set; }

        [DataMember]
        public DateTime Time { get;set;}

        [DataMember]
        public int Duration { get; set; }

        [DataMember]
        public string SportName { get; set; }

        [DataMember]
        public double Calories { get; set; }

        [DataMember]
        public int Minutes { get; set; }

        [DataMember]
        public double Score { get; set; }
    }
}
