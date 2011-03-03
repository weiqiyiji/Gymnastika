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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gymnastika.Widgets.Infrastructure;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Widgets.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string WidgetBoxOpenAnimationKey = "OnWidgetBoxOpen";
        private const string WidgetBoxCloseAnimationKey = "OnWidgetBoxClose";
        private Storyboard _closeWidgetBoxStoryboard;
        private Storyboard _openWidgetBoxStoryboard;

        public MainWindow()
        {
            InitializeComponent();
            IWidgetManager widgetManager = ServiceLocator.Current.GetInstance<IWidgetManager>();
            widgetManager.Add(typeof(DateWidget));
            this.DataContext = new MainViewModel();

            _closeWidgetBoxStoryboard = (Storyboard)Resources[WidgetBoxCloseAnimationKey];
            _openWidgetBoxStoryboard = (Storyboard)Resources[WidgetBoxOpenAnimationKey];
        }
		
        private void OnWidgetBoxButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (widgetBox.Visibility == Visibility.Visible)
            {
                widgetBoxButton.RenderPressed = false;
                _closeWidgetBoxStoryboard.Completed += OnCloseWidgetBoxStoryboard_Completed;
                _closeWidgetBoxStoryboard.Begin();
        	}
        	else
            {
                widgetBoxButton.RenderPressed = true;
                widgetBox.Visibility = System.Windows.Visibility.Visible;
                _openWidgetBoxStoryboard.Completed += OnOpenWidgetBoxStoryboard_Completed;
        	    _openWidgetBoxStoryboard.Begin();
        	}
        }

        private void OnOpenWidgetBoxStoryboard_Completed(object sender, EventArgs e)
        {
            _openWidgetBoxStoryboard.Completed -= OnOpenWidgetBoxStoryboard_Completed;
        }

        private void OnCloseWidgetBoxStoryboard_Completed(object sender, EventArgs e)
        {
            widgetBox.Visibility = System.Windows.Visibility.Collapsed;
            _closeWidgetBoxStoryboard.Completed -= OnCloseWidgetBoxStoryboard_Completed;
        }

        private bool _isDragging;
        private Point _startPosition;

        private void widgetBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPosition = e.GetPosition(null);
        }

        private void widgetBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed && !_isDragging)
            {
                _isDragging = true;
                Point current = e.GetPosition(null);
                
                if (Math.Abs(current.X - _startPosition.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(current.Y - _startPosition.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    DataObject obj = new DataObject(
                        DataFormats.GetDataFormat("Gymnastika").Name, 
                        widgetBox.SelectedItem);
                    System.Windows.DragDrop.DoDragDrop(this, obj, DragDropEffects.Move);
                }
                _isDragging = false;
            }
        }

        private void widgetContainer_Drop(object sender, DragEventArgs e)
        {
            WidgetDescriptor descriptor = (WidgetDescriptor)e.Data.GetData("Gymnastika");
            Point mousePositionInCanvas = e.GetPosition(widgetContainer);

            descriptor.Position = mousePositionInCanvas;
            descriptor.ZIndex = 100;
            descriptor.IsActive = true;
        }

        private Point _targetPosition;

        private void widgetContainer_PreviewDragOver(object sender, DragEventArgs e)
        {
            _targetPosition = e.GetPosition(this.widgetContainer);
            this.positionText.Text = string.Format("x: {0}; y: {1}", _targetPosition.X, _targetPosition.Y);
        }
    }
}
