using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;

namespace Gymnastika.Widgets
{
    public class WidgetContainer : IWidgetContainer
    {
        private ObservableCollection<IWidgetContainerBehavior> _behaviors;

        public WidgetContainer()
        {
            WidgetHosts = new ObservableCollection<IWidgetHost>();
            Widgets = new ObservableCollection<IWidget>();
            _behaviors = new ObservableCollection<IWidgetContainerBehavior>();
            _behaviors.CollectionChanged += OnBehaviorChanged;
        }

        private void OnBehaviorChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IWidgetContainerBehavior behavior in e.NewItems)
                {
                    behavior.Target = this;
                    behavior.Attach();
                }
            }
        }

        public FrameworkElement Target { get; set; }

        public ObservableCollection<IWidgetHost> WidgetHosts { get; private set; }

        public ObservableCollection<IWidget> Widgets { get; private set; }
        
        public IList<IWidgetContainerBehavior> Behaviors
        {
            get { return _behaviors; }
        }
    }
}
