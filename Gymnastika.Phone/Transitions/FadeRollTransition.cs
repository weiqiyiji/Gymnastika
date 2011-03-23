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
using Microsoft.Phone.Controls;

namespace Gymnastika.Phone.Transitions
{
   
    public class FadeRollTransition :TransitionElement
    {
        public FadeRollMode Mode { get; set; }
        public override ITransition GetTransition(UIElement element)
        {
            switch(Mode)
            {
                case FadeRollMode.FadeRollIn:
                    return FadeRollIn.GetTransition(element);
                case FadeRollMode.FadeRollOut:
                    return FadeRollOut.GetTransition(element);
                default:
                    return null;
            }
        }
    }

}
