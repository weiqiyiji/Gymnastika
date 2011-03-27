using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Widgets.Behaviors;
using Gymnastika.Common.Navigation;

namespace Gymnastika.Widgets
{
    public class StateAwareBehavior : WidgetContainerBehaviorBase
    {
        private readonly INavigationService _navigationService;

        public StateAwareBehavior(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        protected override void OnAttach()
        {
            Target.Widgets.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Widgets_CollectionChanged);
            _navigationService.NavigationStart += OnNavigationStart;
            _navigationService.NavigationCompleted += OnNavigationCompleted;
        }

        private void Widgets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (IWidget widget in e.NewItems)
            {
                IStateAware stateAware = widget as IStateAware;
                if (stateAware != null)
                {
                    stateAware.OnResume();
                }
            }
        }

        private void OnNavigationCompleted(object sender, NavigationEventArgs e)
        {
            if (e.TargetDescriptor.ViewName == "WidgetView")
            {
                foreach (IWidget widget in Target.Widgets)
                {
                    IStateAware stateAware = widget as IStateAware;
                    if (stateAware != null)
                    {
                        stateAware.OnResume();
                    }
                }
            }
        }

        private void OnNavigationStart(object sender, NavigationEventArgs e)
        {
            if (e.SourceDescriptor.ViewName == "WidgetView")
            {
                foreach (IWidget widget in Target.Widgets)
                {
                    IStateAware stateAware = widget as IStateAware;
                    if (stateAware != null)
                    {
                        stateAware.OnStop();
                    }
                }
            }
        }
    }
}
