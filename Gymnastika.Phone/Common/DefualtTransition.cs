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
using Gymnastika.Phone.Transitions;

namespace Gymnastika.Phone.Common
{
    public static class DefualtTransition
    {
        public static NavigationInTransition NavigationInTransition{get;set;}
        public static NavigationOutTransition NavigationOutTransition{get;set;}
        public static void SetNavigationTransition(UIElement element)
        {
            TransitionService.SetNavigationInTransition(element, NavigationInTransition);
            TransitionService.SetNavigationOutTransition(element, NavigationOutTransition);
        }
          static DefualtTransition()
          {
              NavigationInTransition = new NavigationInTransition
              {
                  Backward = new SlideTransition { Mode = SlideTransitionMode.SlideDownFadeIn },
                  Forward = new SlideTransition { Mode = SlideTransitionMode.SlideDownFadeIn }
              };
              NavigationOutTransition = new NavigationOutTransition()
              {
                  Backward = new SlideTransition { Mode = SlideTransitionMode.SlideUpFadeOut },
                  Forward = new SlideTransition { Mode = SlideTransitionMode.SlideUpFadeOut }
              };

          }

    }
}
