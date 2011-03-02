using System;
using System.Windows;
using System.Windows.Controls;
using Gymnastika.Widgets.Infrastructure;

namespace Gymnastika.Widgets.Demo
{
    /// <summary>
    /// Interaction logic for DateWidget.xaml
    /// </summary>
    [WidgetMetadata("时间", "/Gymnastika.Widgets.Demo;component/Images/broadcast.png")]
    public partial class DateWidget : UserControl, IWidget
    {
        private bool _isActive;

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
