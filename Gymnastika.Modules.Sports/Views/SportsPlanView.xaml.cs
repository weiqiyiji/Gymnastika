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
using System.ComponentModel;

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

        public bool Expanded
        {
            get { return (bool)GetValue(ExpandedProperty); }
            set { SetValue(ExpandedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Expanded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExpandedProperty =
            DependencyProperty.Register("Expanded", typeof(bool), typeof(SportsPlanView), new UIPropertyMetadata(false));

        

        [Dependency]
        public ISportsPlanViewModel ViewModel
        {
            set { DataContext = value; }
            get { return DataContext as ISportsPlanViewModel; }
        }



        #region ISportsPlanView Members

        public void Expand()
        {
            this.Expanded = true;
        }

        public void Minimize()
        {
            this.Expanded = false;
        }

        #endregion

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("保存完毕！");
        }
    }
    public interface ISportsPlanView
    {
        void Expand();
        void Minimize();
    }

    
}
