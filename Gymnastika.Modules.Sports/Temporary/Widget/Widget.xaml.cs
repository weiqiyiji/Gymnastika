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
using Gymnastika.Widgets;

namespace Gymnastika.Modules.Sports.Temporary.Widget
{
    /// <summary>
    /// Interaction logic for Widget.xaml
    /// </summary>
    public partial class Widget : UserControl , IWidget
    {
        public Widget()
        {
            InitializeComponent();
        }

        #region IWidget Members

        public void Initialize()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
