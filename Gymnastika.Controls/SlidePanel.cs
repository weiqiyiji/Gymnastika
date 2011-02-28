using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Gymnastika.Controls
{
    public class SlidePanel : Panel
    {
        private Size _finalSize;
        private const int InitializeIndex = -1;
        private int _previousSelectedIndex;

        public SlidePanel()
        {
            _previousSelectedIndex = InitializeIndex;
        }

        public int VisibleItemsCount
        {
            get { return (int)GetValue(VisibleItemsCountProperty); }
            set { SetValue(VisibleItemsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisibleItemsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisibleItemsCountProperty =
            DependencyProperty.Register("VisibleItemsCount", typeof(int), typeof(SlidePanel), new UIPropertyMetadata(0));

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(SlidePanel), new FrameworkPropertyMetadata(OnSelectedIndexChanged));

        public double Duration
        {
            get { return (double)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(double), typeof(SlidePanel), new UIPropertyMetadata(0.0));
		
        private static void OnSelectedIndexChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (int.Parse(e.NewValue.ToString()) != InitializeIndex)
            {
                UIElement thiz = (UIElement)sender;
                thiz.InvalidateArrange();
            }
        }

        /// <summary>
        /// Return the shortest distance between the fromIndex and toIndex
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>
        /// A sign int, the sign means direction, positive number means fromIndex should
        /// right shift to the toIndex
        /// </returns>
        protected int CalculateShortestDistance(int from, int to)
        {
            if(from == to) return 0;

            int count = InternalChildren.Count;
            int leftDistance = 0;
            int rightDistance = 0;

            if (from < to)
            {
                leftDistance = from + count - to;
                rightDistance = to - from;
            }
            else
            {
                rightDistance = to + count - from;
                leftDistance = from - to;
            }

            if (leftDistance < rightDistance) return leftDistance;
            else return -rightDistance;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (InternalChildren == null || InternalChildren.Count == 0) return availableSize;
            if (VisibleItemsCount == 0 && InternalChildren.Count > 0) VisibleItemsCount = InternalChildren.Count;

            Size idealSize = new Size();
            Size size = new Size();
            size.Height = availableSize.Height;

            if(InternalChildren == null || InternalChildren.Count == 0)
                size.Width = availableSize.Width;
            else
                size.Width = double.IsPositiveInfinity(availableSize.Width) ? availableSize.Width : availableSize.Width / VisibleItemsCount;          

            foreach(UIElement child in InternalChildren)
            {
                child.Measure(size);
                idealSize.Width += child.DesiredSize.Width;
                idealSize.Height = Math.Max(child.DesiredSize.Height, idealSize.Height);
            }

            return double.IsPositiveInfinity(availableSize.Width) || double.IsPositiveInfinity(availableSize.Height)
                ? idealSize : availableSize;
        }
        
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (InternalChildren == null || InternalChildren.Count == 0) return finalSize;
            _finalSize = finalSize;

            foreach (UIElement child in InternalChildren)
            {
                if (child.RenderTransform as TransformGroup == null)
                {
                    TransformGroup group = new TransformGroup();
                    group.Children.Add(new TranslateTransform());
                    group.Children.Add(new ScaleTransform());
                    child.RenderTransform = group;
                }

                child.Arrange(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));
            }

            Animate();
            _previousSelectedIndex = SelectedIndex;

            return finalSize;
        }

        private void Animate()
        {
            int childrenCount = InternalChildren.Count;
            double childWidth = _finalSize.Width / VisibleItemsCount;
            double x = 0.0, y = 0.0;
            int index = 0;
            int indexSpan = CalculateShortestDistance(_previousSelectedIndex, SelectedIndex);

            foreach (UIElement child in InternalChildren)
            {
                int distance = CalculateShortestDistance(SelectedIndex, index);
                int centerIndex = (VisibleItemsCount - 1) / 2;
                double widthSoFar = childWidth * (centerIndex - distance);

                x = childWidth < child.DesiredSize.Width
                    ? widthSoFar
                    : (widthSoFar + (childWidth - child.DesiredSize.Width) / 2);
                y = _finalSize.Height < child.DesiredSize.Height
                    ? 0.0
                    : (_finalSize.Height - child.DesiredSize.Height) / 2;

                if (_previousSelectedIndex != InitializeIndex)
                {
                    double startX = x - childWidth * indexSpan;
                    TranslateTo(child, startX, y, x, y, Duration);
                }
                else
                {
                    TranslateTo(child, x, y, 0);
                }

                index++;
            }
        }

        private void TranslateTo(UIElement child, double fromX, double fromY, double toX, double toY, double duration)
        {
            TransformGroup group = (TransformGroup)child.RenderTransform;
            TranslateTransform trans = (TranslateTransform)group.Children[0];

            trans.BeginAnimation(TranslateTransform.XProperty, MakeAnimation(fromX, toX, duration));
            trans.BeginAnimation(TranslateTransform.YProperty, MakeAnimation(fromY, toY, duration));
        }

        private void TranslateTo(UIElement child, double x, double y, double duration)
        {
            TransformGroup group = (TransformGroup)child.RenderTransform;
            TranslateTransform trans = (TranslateTransform)group.Children[0];

            trans.BeginAnimation(TranslateTransform.XProperty, MakeAnimation(x, duration));
            trans.BeginAnimation(TranslateTransform.YProperty, MakeAnimation(y, duration));
        }

        private void ScaleTo(UIElement child, double xFactor, double yFactor, double duration)
        {
            TransformGroup group = (TransformGroup)child.RenderTransform;
            ScaleTransform scale = (ScaleTransform)group.Children[1];

            scale.BeginAnimation(ScaleTransform.ScaleXProperty, MakeAnimation(xFactor, duration));
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, MakeAnimation(yFactor, duration));
        }

        private DoubleAnimation MakeAnimation(double from, double to, double duration)
        {
            return MakeAnimation(from, to, duration, null);
        }

        private DoubleAnimation MakeAnimation(double to, double duration)
        {
            return MakeAnimation(to, duration, null);
        }

        private DoubleAnimation MakeAnimation(double from, double to, double duration, EventHandler endEvent)
        {
            DoubleAnimation anim = new DoubleAnimation(from, to, TimeSpan.FromMilliseconds(duration));
            anim.AccelerationRatio = 0.2;
            anim.DecelerationRatio = 0.7;
            if (endEvent != null)
                anim.Completed += endEvent;
            return anim;
        }

        private DoubleAnimation MakeAnimation(double to, double duration, EventHandler endEvent)
        {
            DoubleAnimation anim = new DoubleAnimation(to, TimeSpan.FromMilliseconds(duration));
            anim.AccelerationRatio = 0.2;
            anim.DecelerationRatio = 0.7;
            if (endEvent != null)
                anim.Completed += endEvent;
            return anim;
        }
    }
}
