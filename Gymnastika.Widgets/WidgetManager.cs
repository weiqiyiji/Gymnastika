using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using Gymnastika.Common.Extensions;
using Gymnastika.Widgets;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Widgets
{
    public class WidgetManager : IWidgetManager
    {
        private readonly IWidgetContainerAccessor _containerAccessor;

        public WidgetManager(IWidgetContainerAccessor containerAccessor)
        {
            _containerAccessor = containerAccessor;
            InternalWidgets = new List<IWidget>();
        }

        private IList<IWidget> InternalWidgets { get; set; }

        public ReadOnlyCollection<IWidget> Widgets
        {
            get { return InternalWidgets.ToReadOnlyCollection(); }
        }

        public void Add(IWidget widget)
        {
            if(widget == null)
            {
                throw new ArgumentNullException("widget");
            }

            if(_containerAccessor.Container == null)
            {
                throw new InvalidOperationException("WidgetContainer hasn't set");
            }

            InternalWidgets.Add(widget);
            _containerAccessor.Container.Widgets.Add(widget);
        }

        public void Remove(IWidget widget)
        {
            throw new NotImplementedException();
        }
    }
}
