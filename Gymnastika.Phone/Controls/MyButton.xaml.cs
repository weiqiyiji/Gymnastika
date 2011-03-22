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
using System.Windows.Media.Imaging;

namespace Gymnastika.Phone.Controls
{
    public partial class MyButton : UserControl
    {

        public enum ButtonStyle:int
        {
            Button = 0,
            ComboLeft = 1,
            ComboRight = 2
        }
        public enum ButtonStatus:int
        {
            Normal=0,
            Hover = 1,
            Pressed = 2
        }
       private ButtonStyle m_style = ButtonStyle.Button;
       
       public ButtonStyle  ButtonBackgroundStyle
        {
            get { return m_style; }
            set { m_style = value; SetButtonStatus(this.Status); }
        }
        public ButtonStatus Status { get; protected set; }
        public ImageSource ImageNormal { get; set; }
        public ImageSource ImageHover { get; set; }
        public ImageSource ImageSelected { get; set; }
        static ImageSource[,] Images;
        public MyButton()
        {
            if (Images == null)
            {
               
                Images = new ImageSource[3, 3];
                Images[0, 0] = LoadImage("/Images/button-back.png");
                Images[0, 1] = LoadImage("/Images/button-hover.png");
                Images[0, 2] = LoadImage("/Images/button-selected.png");

                Images[1, 0] = LoadImage("/Images/combobutton-left.png");
                Images[1, 1] = LoadImage("/Images/combobutton-left-hover.png");
                Images[1, 2] = LoadImage("/Images/combobutton-left-selected.png");

                Images[2, 0] = LoadImage("/Images/combobutton-right.png");
                Images[2, 1] = LoadImage("/Images/combobutton-right-hover.png");
                Images[2, 2] = LoadImage("/Images/combobutton-right-selected.png");
            }

            InitializeComponent();
            this.Width = 27;
            this.Height = 23;
            SetButtonStatus(ButtonStatus.Normal);
        }
        private void SetButtonStatus(ButtonStatus status)
        {
            ImageBrush b =(ImageBrush) this.LayoutRoot.Background;
            this.Status = status;
            this.LayoutRoot.Background = new ImageBrush() { ImageSource = Images[(int)this.ButtonBackgroundStyle, (int)status], Opacity = 1, Stretch = Stretch.Fill };
        }
        private static ImageSource LoadImage(string Source)
        {
            Source = "/Gymnastika.Phone;component/" + Source;
            return LoadImage(new Uri(Source, UriKind.Relative));
        }
        private static ImageSource LoadImage(Uri Source)
        {
            return new BitmapImage(Source);
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(MyButton_ManipulationStarted);
            this.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(MyButton_ManipulationCompleted);
            SetButtonStatus(ButtonStatus.Normal);
        }

        void MyButton_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            SetButtonStatus(ButtonStatus.Normal);
        }

        void MyButton_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            SetButtonStatus(ButtonStatus.Pressed);
        }
    }
}
