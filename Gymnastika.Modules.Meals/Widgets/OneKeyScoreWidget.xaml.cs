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
using Gymnastika.Modules.Meals.ViewModels;

namespace Gymnastika.Modules.Meals.Widgets
{
    /// <summary>
    /// Interaction logic for OneKeyScoreWidget.xaml
    /// </summary>
    [WidgetMetadata("一键打分", "/Gymnastika.Modules.Meals;component/Images/一键打分.png")]
    public partial class OneKeyScoreWidget : IWidget
    {
        public OneKeyScoreWidget(OneKeyScoreWidgetModel model)
        {
            InitializeComponent();

            Model = model;
        }

        public OneKeyScoreWidgetModel Model
        {
            get
            {
                return this.DataContext as OneKeyScoreWidgetModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        #region IWidget Members

        public void Initialize()
        {
        }

        #endregion
    }
}
