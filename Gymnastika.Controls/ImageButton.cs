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

namespace Gymnastika.Controls
{
    public enum ImageButtonMode
    { 
        Single,
        Switchover
    }


    public class ImageButton : Button
    {
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));



        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ImageButton), new UIPropertyMetadata(new CornerRadius(0)));

        

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Stretch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ImageButton), new UIPropertyMetadata(Stretch.None));

        public ImageButtonMode ImageMode
        {
            get { return (ImageButtonMode)GetValue(ImageModeProperty); }
            set { SetValue(ImageModeProperty, value); }
        }

        public static readonly DependencyProperty ImageModeProperty =
            DependencyProperty.Register("ImageMode", typeof(ImageButtonMode), typeof(ImageButton), new UIPropertyMetadata(ImageButtonMode.Single));

        public ImageSource MouseOverImageSource
        {
            get { return (ImageSource)GetValue(MouseOverImageSourceProperty); }
            set { SetValue(MouseOverImageSourceProperty, value); }
        }

        public static readonly DependencyProperty MouseOverImageSourceProperty =
            DependencyProperty.Register("MouseOverImageSource", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));

        public ImageSource MousePressedImageSource
        {
            get { return (ImageSource)GetValue(MousePressedImageSourceProperty); }
            set { SetValue(MousePressedImageSourceProperty, value); }
        }

        public static readonly DependencyProperty MousePressedImageSourceProperty =
            DependencyProperty.Register("MousePressedImageSource", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));

        
    }
}
