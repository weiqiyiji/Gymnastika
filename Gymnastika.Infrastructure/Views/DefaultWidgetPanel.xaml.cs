using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Gymnastika.ViewModels;
using Gymnastika.Widgets;

namespace Gymnastika.Views
{
    /// <summary>
    /// Interaction logic for DefaultWidgetPanel.xaml
    /// </summary>
    public partial class DefaultWidgetPanel
    {
        private const string WidgetBoxOpenAnimationKey = "OnWidgetBoxOpen";
        private const string WidgetBoxCloseAnimationKey = "OnWidgetBoxClose";
        private const string WidgetDataFormat = "Gymnastika.Widgets";
        private Storyboard _closeWidgetBoxStoryboard;
        private Storyboard _openWidgetBoxStoryboard;
        private bool _isDragging;
        private Point _startPosition;

        public DefaultWidgetPanel(WidgetPanelViewModel model)
        {
            InitializeComponent();
            Model = model;
            _closeWidgetBoxStoryboard = (Storyboard)Resources[WidgetBoxCloseAnimationKey];
            _openWidgetBoxStoryboard = (Storyboard)Resources[WidgetBoxOpenAnimationKey];
        }

        public WidgetPanelViewModel Model
        {
            get { return DataContext as WidgetPanelViewModel; }
            set { DataContext = value; }
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

        private void widgetBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPosition = e.GetPosition(null);
        }

        private void widgetBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !_isDragging)
            {
                _isDragging = true;
                Point current = e.GetPosition(null);

                if (Math.Abs(current.X - _startPosition.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(current.Y - _startPosition.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    if (widgetBox.SelectedItem != null)
                    {
                        DataObject obj = new DataObject(
                            DataFormats.GetDataFormat(WidgetDataFormat).Name,
                            widgetBox.SelectedItem);
                        DragDrop.DoDragDrop(this, obj, DragDropEffects.Move);
                    }
                }
                _isDragging = false;
            }
        }

        private void OnWidgetDrop(object sender, DragEventArgs e)
        {
            WidgetDescriptor descriptor = (WidgetDescriptor)e.Data.GetData(WidgetDataFormat);
            Point mousePositionInCanvas = e.GetPosition(widgetContainer);

            descriptor.Position = mousePositionInCanvas;
            descriptor.ZIndex = 100;
            descriptor.IsActive = true;
        }
    }
}
