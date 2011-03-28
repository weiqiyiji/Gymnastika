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
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Sync.Communication.Client;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Modules.Sports.Communication.Services;

namespace Gymnastika.Modules.Sports.Widget
{
    /// <summary>
    /// Interaction logic for DailySportWidget.xaml
    /// </summary>
    [WidgetMetadata("每日运动计划", "/Gymnastika.Modules.Sports;component/Resources/Images/Sport.png")]
    public partial class DailySportWidget : UserControl , IWidget
    {
        DailySportViewModel _model;
        public DailySportWidget(DailySportViewModel viewmodel)
        {
            InitializeComponent();
            _model = viewmodel;
        }

        #region IWidget Members

        public void Initialize()
        {
            DataContext = _model;
            _model.Run();
        }

        #endregion

    }
}
