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
using System.Windows.Shapes;
using Gymnastika.ViewModels;
using Microsoft.Practices.Unity;

namespace Gymnastika.Views
{
    /// <summary>
    /// Interaction logic for LogOnView.xaml
    /// </summary>
    public partial class LogOnView : Window, ILogOnView
    {
        public LogOnView()
        {
            InitializeComponent();
        }

        [Dependency]
        public LogOnViewModel Model
        {
            get
            {
                return DataContext as LogOnViewModel;
            }
            set
            {
                DataContext = value;
            }
        }
    }
}
