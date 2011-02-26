using System;
using System.Windows.Controls;

namespace Gymnastika.Widgets.Demo
{
    /// <summary>
    /// Interaction logic for DateWidget.xaml
    /// </summary>
    public partial class DateWidget : UserControl, IWidget
    {
        public DateWidget()
        {
            InitializeComponent();
        }

        public bool IsActive { get; set; }
        public event EventHandler IsActiveChanged;

        public void Initialize()
        {
            dateLabel.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
