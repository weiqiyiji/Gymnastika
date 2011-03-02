using System;
using System.Windows;
using System.Windows.Controls;
using Gymnastika.Widgets.Infrastructure;

namespace Gymnastika.Widgets
{
    /// <summary>
    /// Interaction logic for DateWidget.xaml
    /// </summary>
    [WidgetMetadata("时间", "/Gymnastika.Infrastructure;component/Images/datewidget_icon.png")]
    public partial class DateWidget : UserControl, IWidget
    {
        public DateWidget()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            dateLabel.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
