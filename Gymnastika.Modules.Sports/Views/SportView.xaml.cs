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
using Gymnastika.Modules.Sports.ViewModels;
using Microsoft.Practices.Unity;

namespace Gymnastika.Modules.Sports.Views
{
    /// <summary>
    /// Interaction logic for SportView.xaml
    /// </summary>
    public partial class SportView : Window, ISportView
    {
        public SportView()
        {
            InitializeComponent();
        }

        [Dependency]
        public ISportViewModel ViewModel
        {
            get { return DataContext as ISportViewModel; }
            set { DataContext = value; }
        }

        private void surfaceButton1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public interface ISportView
    {

    }
}
