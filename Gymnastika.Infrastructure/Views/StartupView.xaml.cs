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
using System.Windows.Controls.Primitives;

namespace Gymnastika.Views
{
    /// <summary>
    /// Interaction logic for StartupView.xaml
    /// </summary>
    public partial class StartupView : UserControl, IStartupView
    {


        public static bool GetIsCenter(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsCenterProperty);
        }

        public static void SetIsCenter(DependencyObject obj, bool value)
        {
            obj.SetValue(IsCenterProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsCenter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCenterProperty =
            DependencyProperty.RegisterAttached("IsCenter", typeof(bool), typeof(StartupView), new UIPropertyMetadata(false));

        

        public StartupView(StartupViewModel model)
        {
            InitializeComponent();
            Model = model;
            this.Loaded += new RoutedEventHandler(StartupView_Loaded);
        }

        void StartupView_Loaded(object sender, RoutedEventArgs e)
        {
        }    

        public StartupViewModel Model
        {
            get { return DataContext as StartupViewModel; }
            set { DataContext = value; }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector selector = (Selector)sender;

            if (e.AddedItems.Count == 1)
            {
                var container = selector.ItemContainerGenerator.ContainerFromItem(e.AddedItems[0]);
                SetIsCenter(container, true);
            }

            if (e.RemovedItems.Count == 1)
            {
                var oldContainer = selector.ItemContainerGenerator.ContainerFromItem(e.RemovedItems[0]);
                SetIsCenter(oldContainer, false);
            }
        }

        private void OnSelectorClick(object sender, RoutedEventArgs e)
        {
            Selector selector = (Selector)sender;
            DependencyObject container = selector.ItemContainerGenerator.ContainerFromIndex(selector.SelectedIndex);
            bool isCenter = GetIsCenter(container);

            if (isCenter)
            {
                if (Model.LogOnCommand.CanExecute(null))
                    Model.LogOnCommand.Execute(null);
            }
            else
            {
                SetIsCenter(container, true);
            }
        }
    }
}
