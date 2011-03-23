using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gymnastika.Phone.Controls
{
    public partial class ImageButton : UserControl
    {
        public ImageSource ImageNormal { get; set; }
        public ImageSource ImageDown { get; set; }
        public ImageSource ImageHover { get; set; }
        public ImageButton()
        {
            InitializeComponent();
            this.MouseEnter += new MouseEventHandler(ImageButton_MouseEnter);
            this.MouseLeave += new MouseEventHandler(ImageButton_MouseLeave);
            this.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(ImageButton_ManipulationStarted);
            this.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(ImageButton_ManipulationCompleted);
        }

        void ImageButton_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            ShowImage(ImageNormal);    
        }

        void ImageButton_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            if (ImageDown != null)
                ShowImage(ImageDown);
            else
                ShowImage(ImageNormal);
        }

        void ImageButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ShowImage(ImageNormal);    
        }
        void ShowImage(ImageSource source)
        {
            if (source == null) return;
        }
        void ImageButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (ImageHover == null)
                ShowImage(ImageNormal);
            else
                ShowImage(ImageHover);
        }
    }
}
