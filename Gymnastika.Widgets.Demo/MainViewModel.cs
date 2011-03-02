using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Widgets.Demo
{
    public class MainViewModel : NotificationObject
    {
        private readonly IWidgetManager _widgetManager;

        public MainViewModel()
        {
            _widgetManager = ServiceLocator.Current.GetInstance<IWidgetManager>();
        }

        public ObservableCollection<WidgetDescriptor> Widgets
        {
            get { return _widgetManager.Descriptors; }
        }

        private WidgetDescriptor _selectedWidget;

        public WidgetDescriptor SelectedWidget
        {
            get { return _selectedWidget; }
            set
            {
                if (_selectedWidget != value)
                {
                    _selectedWidget = value;
                    RaisePropertyChanged("SelectedWidget");
                }
            }
        }
    }
}
