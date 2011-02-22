using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gymnastika.Controls
{
    public class CheckButtonGroupItem : ListBoxItem
    {
        private CheckButtonGroup _parent;

        static CheckButtonGroupItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(CheckButtonGroupItem), new FrameworkPropertyMetadata(typeof(CheckButtonGroupItem)));
        }

        public CheckButtonGroupItem(CheckButtonGroup parent)
        {
            _parent = parent;
        }

        public int Index { get; set; }

        public CornerRadius ComputedCornerRadius
        {
            get
            {
                if (Index == 0)
                {
                    return new CornerRadius(CornerRadius, 0, 0, CornerRadius);
                }

                return _parent.Items.Count - 1 == Index 
                    ? new CornerRadius(0, CornerRadius, CornerRadius, 0) 
                    : new CornerRadius(0.0);
            }
        }

        public Thickness ComputedBorderThickness
        {
            get
            {
                if(Index == 0)
                {
                    return new Thickness(SingleBorderThickness, SingleBorderThickness, 0, SingleBorderThickness);
                }

                return _parent.Items.Count - 1 == Index
                    ? new Thickness(0, SingleBorderThickness, SingleBorderThickness, SingleBorderThickness)
                    : new Thickness(0, SingleBorderThickness, 0, SingleBorderThickness); 
            }
        }


        public double SingleBorderThickness
        {
            get { return (double)GetValue(SingleBorderThicknessProperty); }
            set { SetValue(SingleBorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SingleBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SingleBorderThicknessProperty =
            DependencyProperty.Register("SingleBorderThickness", typeof(double), typeof(CheckButtonGroupItem), new UIPropertyMetadata(0.0));

        
        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(double), typeof(CheckButtonGroupItem), new UIPropertyMetadata(0.0));

        public Brush SelectedBackground
        {
            get { return (Brush)GetValue(SelectedBackgroundProperty); }
            set { SetValue(SelectedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty SelectedBackgroundProperty =
            DependencyProperty.Register("SelectedBackground", typeof(Brush), typeof(CheckButtonGroupItem), null);

        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseOverBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(CheckButtonGroupItem), null);

          
    }
}
