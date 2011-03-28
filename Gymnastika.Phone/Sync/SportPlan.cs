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
    public class SportPlan
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public List<Sport> Sports { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
    }
}
