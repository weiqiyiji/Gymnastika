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
using GongSolutions.Wpf.DragDrop.Utilities;

namespace Gymnastika.Views
{
    /// <summary>
    /// Interaction logic for StartupView.xaml
    /// </summary>
    public partial class StartupView : UserControl, IStartupView
    {


        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsFocused.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached("IsFocused", typeof(bool), typeof(StartupView), new UIPropertyMetadata(false));

        

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

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox list = (ListBox) sender;

            if (e.AddedItems.Count == 1)
            {
                UIElement container = (UIElement) list.ItemContainerGenerator.ContainerFromItem(e.AddedItems[0]);
                container.MouseLeftButtonUp += container_MouseLeftButtonUp;

                if(e.RemovedItems.Count == 0)
                    SetIsFocused(container, true);
            }

            if (e.RemovedItems.Count == 1)
            {
                UIElement oldContainer = (UIElement) list.ItemContainerGenerator.ContainerFromItem(e.RemovedItems[0]);
                SetIsFocused(oldContainer, false);

                oldContainer.MouseLeftButtonUp -= container_MouseLeftButtonUp;
            }
        }

        private void container_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UIElement element = (UIElement) sender;
            bool isFocused = GetIsFocused(element);

            if (isFocused)
            {
                if (Model.LogOnCommand.CanExecute(null))
                    Model.LogOnCommand.Execute(null);
            }
            else
            {
                SetIsFocused(element, true);
            }
        }
    }
}
