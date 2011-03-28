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

namespace Gymnastika.Phone.Sync
{
    [DataContract(Namespace="")]
    public class Food
    {
        [DataMember]
        public double Calorie { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double Amount { get; set; }
    }
}
