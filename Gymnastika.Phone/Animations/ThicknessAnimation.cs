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

namespace Gymnastika.Phone.Animations
{
    public class ThicknessAnimation:Timeline
    {
        public Thickness From { get; set; }
        public Thickness To { get; set; }
       
    }
}
