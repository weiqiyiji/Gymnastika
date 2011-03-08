using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gymnastika.Widgets.Views
{
    public partial class WidgetHost : IWidgetHost
    {
        private WidgetDescriptor _descriptor;
        private IWidget _widget;

        public WidgetHost(IWidgetManager widgetManager)
        {
            WidgetManager = widgetManager;
            InitializeComponent();
        }

        //TODO find the usage of Id
        public int Id { get; set; }

        public IWidgetManager WidgetManager { get; set; }

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

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            _descriptor.IsActive = false;
        }
    }
}
