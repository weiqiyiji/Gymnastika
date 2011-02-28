using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gymnastika.Widgets.Views
{
    /// <summary>
    /// Interaction logic for WidgetHost.xaml
    /// </summary>
    public partial class WidgetHost : UserControl, IWidgetHost
    {
        private WidgetDescriptor _descriptor;
        private IWidget _widget;

        public WidgetHost(IWidgetManager widgetManager)
        {
            WidgetManager = widgetManager;
            InitializeComponent();
            Expand();
        }

        //TODO find the usage of Id
        public int Id { get; set; }

        public IWidgetManager WidgetManager { get; set; }

        public WidgetState State { get; private set; }

        public IWidget Widget
        {
            get { return _widget; }
            set
            {
                if(_widget != value)
                {
                    _widget = value;
                    if (_widget != null)
                    {
                        _descriptor = WidgetManager.Descriptors.Single(x => x.WidgetType == _widget.GetType());
                    }
                }
            }
        }

        public void Expand()
        {
            State = WidgetState.Expanded;
        }

        public void Collapse()
        {
            State = WidgetState.Collapsed;
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            _descriptor.IsActive = false;
        }
    }
}
