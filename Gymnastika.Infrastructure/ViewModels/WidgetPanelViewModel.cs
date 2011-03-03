using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Gymnastika.Widgets;

namespace Gymnastika.ViewModels
{
    public class WidgetPanelViewModel
    {
        private readonly IWidgetManager _widgetManager;

        public WidgetPanelViewModel(IWidgetManager widgetManager)
        {
            _widgetManager = widgetManager;
            _widgetManager.Add(typeof(DateWidget));
        }

        public ObservableCollection<WidgetDescriptor> Widgets
        {
            get { return _widgetManager.Descriptors; }
        }
    }
}
