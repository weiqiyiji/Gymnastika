using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Gymnastika.Widgets.Behaviors
{
    public class DragWidgetBehavior : IWidgetContainerBehavior
    {
        public const string BehaviorKey = "DragWidgetBehavior";

        internal static Point GetStartPosition(DependencyObject obj)
        {
            return (Point)obj.GetValue(StartPositionProperty);
        }

        internal static void SetStartPosition(DependencyObject obj, Point value)
        {
            obj.SetValue(StartPositionProperty, value);
        }

        public static readonly DependencyProperty StartPositionProperty =
            DependencyProperty.RegisterAttached("StartPosition", typeof(Point), typeof(DragWidgetBehavior), null);

        public IWidgetContainer Target { get; set; }

        public void Attach()
        {
            if (Target.Target is Canvas)
            {
                Target.WidgetHosts.CollectionChanged += OnWidgetHostsCollectionChanged;
            }
        }

        private void OnWidgetHostsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (UIElement item in e.NewItems)
                {
                    item.MouseLeftButtonDown += OnWidgetHostMouseLeftButtonDown;
                }
            }

            if(e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (UIElement item in e.OldItems)
                {
                    item.MouseDown -= OnWidgetHostMouseLeftButtonDown;
                }
            }
        }

        private void OnWidgetHostMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var element = (UIElement)sender;
            var transform = (TranslateTransform) element.RenderTransform;
            element.MouseMove -= OnWidgetHostMouseMove;
            element.MouseLeftButtonUp -= OnWidgetHostMouseLeftButtonUp;
            Canvas.SetTop(element, Canvas.GetTop(element) + transform.Y);
            Canvas.SetLeft(element, Canvas.GetLeft(element) + transform.X);
            transform.X = 0;
            transform.Y = 0;
            element.ReleaseMouseCapture();
        }

        private void OnWidgetHostMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = (UIElement) sender;
            SetStartPosition(element, e.GetPosition(Target.Target));
            element.MouseMove += OnWidgetHostMouseMove;
            element.MouseLeftButtonUp += OnWidgetHostMouseLeftButtonUp;
            element.CaptureMouse();
        }

        private void OnWidgetHostMouseMove(object sender, MouseEventArgs e)
        {
            var element = (UIElement)sender;
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                if (element.RenderTransform == null || !(element.RenderTransform is TranslateTransform))
                {
                    element.RenderTransform = new TranslateTransform();
                }

                var startPosition = GetStartPosition(element);
                var currentPositon = e.GetPosition(Target.Target);
                TranslateTransform translateTransform = (TranslateTransform)element.RenderTransform;

                translateTransform.X = currentPositon.X - startPosition.X;
                translateTransform.Y = currentPositon.Y - startPosition.Y;
            }
        }
    }
}
