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
        public WidgetHost()
        {
            InitializeComponent();
            Expand();
        }

        #region IWidgetHost Members

        //TODO find the usage of Id
        public int Id { get; set; }

        public void Expand()
        {
            State = WidgetState.Expanded;
        }

        public void Collapse()
        {
            State = WidgetState.Collapsed;
        }

        public WidgetState State { get; private set; }

        public IWidget Widget { get; set; }

        public bool IsActive
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public event EventHandler IsActiveChanged;

        #endregion
    }
}
