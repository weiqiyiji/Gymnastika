using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Widgets.Demo
{
    public class MainViewModel
    {
        private IWidgetManager _widgetManager;

        public MainViewModel()
        {
            _widgetManager = ServiceLocator.Current.GetInstance<IWidgetManager>();
            //_widgetManager.Add();
        }

        public ObservableCollection<IWidget> Widgets
        {
            get { return new ObservableCollection<IWidget>(_widgetManager.Widgets); }
        }
    }
}
