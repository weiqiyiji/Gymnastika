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
using Microsoft.Practices.Unity;
using Gymnastika.Modules.Sports.ViewModels;

namespace Gymnastika.Modules.Sports.Views
{
    /// <summary>
    /// Interaction logic for SportPlanView.xaml
    /// </summary>
    public partial class SportsPlanView : UserControl, ISportsPlanView
    {
        public SportsPlanView()
        {
            InitializeComponent();
        }

        [Dependency]
        public ISportsPlanViewModel ViewModel
        {
            set { DataContext = value; }
            get { return DataContext as ISportsPlanViewModel; }
        }
    }
}
