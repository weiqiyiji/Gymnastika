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
using Gymnastika.ViewModels;
using Gymnastika.Views;

namespace Gymnastika.Views
{
    /// <summary>
    /// Interaction logic for StartupView.xaml
    /// </summary>
    public partial class StartupView : UserControl, IStartupView
    {
        public StartupView(StartupViewModel model)
        {
            InitializeComponent();
            Model = model;
        }    

        public StartupViewModel Model
        {
            get { return DataContext as StartupViewModel; }
            set { DataContext = value; }
        }
    }
}
