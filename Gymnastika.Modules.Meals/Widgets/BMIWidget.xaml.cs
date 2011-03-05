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

namespace Gymnastika.Modules.Meals.Widgets
{
    /// <summary>
    /// Interaction logic for BMIWidget.xaml
    /// </summary>
    [WidgetMetadata("BMI", "/Gymnastika.Modules.Meals;component/Images/BMI.jpg")]
    public partial class BMIWidget : IWidget
    {
        public BMIWidget()
        {
            InitializeComponent();
        }

        #region IWidget Members

        public void Initialize()
        {
        }

        #endregion
    }
}
