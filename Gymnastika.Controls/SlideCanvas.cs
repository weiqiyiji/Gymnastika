using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Gymnastika.Controls
{
    public class SlideCanvas : Canvas
    {
        private Point _startPosition;
        private Point _lastMousePosition;
        private bool _isMouseDown;
        private const double Decay = 0.93;
        private const double MouseDownDecay = 0.99;
        private const double SpeedSpringness = 0.4;
        private const double BouncingSpringness = 0.08;
        private const double VelocityLimit = 0.001;

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(SlideCanvas), new UIPropertyMetadata(Orientation.Horizontal));
        
        public SlideCanvas()
        {
            this.MouseLeftButtonDown += SlideCanvas_MouseLeftButtonDown;
        }

        public double CurrentVelocity
        {
            get { return (double)GetValue(CurrentVelocityProperty); }
            set { SetValue(CurrentVelocityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentVelocity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentVelocityProperty =
            DependencyProperty.Register("CurrentVelocity", typeof(double), typeof(SlideCanvas), new UIPropertyMetadata(0.0));

        
        private void SlideCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (InternalChildren != null && InternalChildren.Count > 0)
            {
                CurrentVelocity = 0;
                _isMouseDown = true;
                _startPosition = e.GetPosition(this);
                _lastMousePosition = e.GetPosition(this);
                this.MouseMove += SlideCanvas_MouseMove;
                this.MouseLeftButtonUp += SlideCanvas_MouseLeftButtonUp;
                CompositionTarget.Rendering += CompositionTarget_Rendering;
            }
        }

        private void SlideCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
            this.MouseMove -= SlideCanvas_MouseMove;
            this.MouseLeftButtonUp -= SlideCanvas_MouseLeftButtonUp;
        }

        private void SlideCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                UIElement element = InternalChildren[0];
                Point currentPosition = e.GetPosition(this);
                double deltaDistance = 0;
                if (currentPosition != _lastMousePosition)
                {
                    if (Orientation == Orientation.Horizontal)
                    {
                        deltaDistance = currentPosition.X - _lastMousePosition.X;
                        Canvas.SetLeft(element, Canvas.GetLeft(element) + deltaDistance);
                    }
                    if (Orientation == Orientation.Vertical)
                    {
                        deltaDistance = currentPosition.Y - _lastMousePosition.Y;
                        Canvas.SetTop(element, Canvas.GetTop(element) + deltaDistance);
                    }

                    _lastMousePosition = currentPosition;
                    CurrentVelocity += (SpeedSpringness * deltaDistance);
                }
            }
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            //if (CurrentVelocity < VelocityLimit)
            //{
            //    CompositionTarget.Rendering -= CompositionTarget_Rendering;
            //    return;
            //}

            //if (_isMouseDown)
            //    CurrentVelocity *= MouseDownDecay;
            //else 
            

            if (!_isMouseDown)
            {
                CurrentVelocity *= Decay;
                FrameworkElement scrollElement = (FrameworkElement)InternalChildren[0];
                
                double bouncing = 0;

                //if (y > 0)
                //{
                //    bouncing = -y * BouncingSpringness;
                //}
                //else if (y + textHeight < canvasHeight)
                //{
                //    bouncing = (canvasHeight - textHeight - y) * BOUNCING_SPRINGESS;
                //}

                if(Orientation == Orientation.Horizontal)
                {
                    double x = Canvas.GetLeft(scrollElement);
                    //if (x > 0)
                    //    bouncing = -x * BouncingSpringness;
                    //else if (x + scrollElement.ActualWidth < this.ActualWidth)
                    //    bouncing = (this.ActualWidth - scrollElement.ActualWidhth - x) * BouncingSpringness;

                    Canvas.SetLeft(scrollElement, x + CurrentVelocity + bouncing);
                }
                else
                {
                    double y = Canvas.GetTop(scrollElement);
                    //if (y > 0)
                    //    bouncing = -y * BouncingSpringness;
                    //else if (y + scrollElement.ActualHeight < this.ActualHeight)
                    //    bouncing = (this.ActualHeight - scrollElement.ActualHeight - y) * BouncingSpringness;
                    Canvas.SetTop(scrollElement, y + CurrentVelocity + bouncing);
                }

                if (Math.Abs(CurrentVelocity) < VelocityLimit)
                {
                    CompositionTarget.Rendering -= CompositionTarget_Rendering;
                }
            }
        }
    }
}
