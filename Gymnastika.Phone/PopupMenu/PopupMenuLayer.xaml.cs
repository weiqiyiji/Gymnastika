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
using Microsoft.Phone.Controls;

namespace Gymnastika.Phone.PopupMenu
{
    public partial class PopupMenuLayer : UserControl
    {
        public delegate void MenuClickHandler(object sender, int MenuID);
        public event MenuClickHandler MenuClick;
        public double  MenuTotalHight { get;private set; }
        public string Title { get { return txtTitle.Text; } set{txtTitle.Text=value;} }
        private int m_ItemsCount;
        private TimeSpan m_ItemDelay = TimeSpan.FromSeconds(.3);
        Storyboard ShowItemsStoryBoard = new Storyboard();
        public PopupMenuLayer()
        {
            MenuTotalHight = 50;
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PopupMenuLayer_Loaded);
            
        }

        void PopupMenuLayer_Loaded(object sender, RoutedEventArgs e)
        {
            AdaptSize();
            ShowItemsStoryBoard.Completed += new EventHandler(ShowItemsStoryBoard_Completed);
        }
         
        void ShowItemsStoryBoard_Completed(object sender, EventArgs e)
        {  ShowItemsStoryBoard.Stop();
            ShowItemsStoryBoard.Children.Clear();
            foreach (UIElement ue in MenuContainer.Children)
            {
                ue.Projection = null;
                ue.Opacity = 1;
            }
        }


        public void AddMenu(int ID,ImageSource Image,string Text)
        {
            PopupMenuItem item = new PopupMenuItem() { Image = Image, Text = Text };
            var plane = new PlaneProjection();
            item.Projection = plane;
            plane.LocalOffsetX = 100;
            DoubleAnimation ani = new DoubleAnimation();
            DoubleAnimation ani2 = new DoubleAnimation();
            ani2.From = 0;
            ani2.To = 1;
            ani2.Duration = TimeSpan.FromSeconds(.5);
            Storyboard.SetTarget(ani2, item);
            Storyboard.SetTargetProperty(ani2, new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTarget(ani, plane);
            Storyboard.SetTargetProperty(ani, new PropertyPath(PlaneProjection.LocalOffsetXProperty));
            ani.Duration = TimeSpan.FromSeconds(0.2);
            ani.From = plane.LocalOffsetX;
            ani.To = 0;
            m_ItemsCount++;
            ani2.Duration=   ani.Duration= TimeSpan.FromSeconds(m_ItemDelay.TotalSeconds * m_ItemsCount);
            ShowItemsStoryBoard.Children.Add(ani);
            ShowItemsStoryBoard.Children.Add(ani2);
            item.Opacity = 0;
            item.Click += new PopupMenuItem.ClickHandler(item_Click);
            item.ID = ID;
            MenuContainer.Children.Add(item); 
            MenuTotalHight += item.Height;
        }

        void item_Click(object sender, MouseButtonEventArgs e)
        {
            PopupMenuItem item = sender as PopupMenuItem;
            if (MenuClick != null)
                MenuClick(this, item.ID);
        }
        private void AdaptSize()
        {
            PhoneApplicationFrame _rootVisual = (App.Current.RootVisual as PhoneApplicationFrame);
           
            bool portrait = PageOrientation.Portrait == (PageOrientation.Portrait & _rootVisual.Orientation);
               double width = portrait ? _rootVisual.ActualWidth : _rootVisual.ActualHeight;
               double height = portrait ? _rootVisual.ActualHeight : _rootVisual.ActualWidth;

            this.VerticalAlignment = VerticalAlignment.Center;
            this.HorizontalAlignment = HorizontalAlignment.Center;


            if (portrait)
            {
                this.Width = width-50;
                this.Height = MenuTotalHight + 10 < height - 100 ? MenuTotalHight + 10 : height - 50;
                this.SetValue(Canvas.LeftProperty,width/2-this.Width/2);
                this.SetValue(Canvas.TopProperty, (height-50) / 2 - this.Height / 2 + 50);
            }
            else
            {
                this.Height = MenuTotalHight + 10 < height ? MenuTotalHight + 10 : height-10;
                this.SetValue(Canvas.TopProperty, (height - 10) / 2 - this.Height/2 + 10);
                this.Width = width - 100;
                if (_rootVisual.Orientation == PageOrientation.LandscapeLeft)
                {
                    SetValue(Canvas.LeftProperty, 60 + (width - 50) / 2 - this.Width / 2) ;
                }
                else
                    SetValue(Canvas.LeftProperty,10.0);
            }
            MenuContainer.Height = MenuTotalHight - 44;
        
            if (firstShow)
                Show();
            firstShow = false;  
        }
        bool firstShow = true;
        public void Show()
        {
          
            //this.RenderTransform = new ScaleTransform()
            //{
            //    CenterX = this.Width / 2,
            //    CenterY = this.Height / 2
            //};
            //Storyboard sb = new Storyboard();
            //double from = 0.1;
            //double to = 1;
            //DoubleAnimation anix = new DoubleAnimation();
            //DoubleAnimation aniy = new DoubleAnimation();
            //anix.From = aniy.From = from;
            //anix.To = aniy.To = to;
            //anix.Duration = aniy.Duration = TimeSpan.FromSeconds(0.3);
            //Storyboard.SetTarget(anix, this.RenderTransform);
            //Storyboard.SetTarget(aniy, this.RenderTransform);
            //Storyboard.SetTargetProperty(anix, new PropertyPath(ScaleTransform.ScaleXProperty));
            //Storyboard.SetTargetProperty(aniy, new PropertyPath(ScaleTransform.ScaleYProperty));
            //sb.Children.Add(anix);
            //sb.Children.Add(aniy);
            //sb.Completed += new EventHandler(sb_Completed);
            //sb.Begin();

            this.IsEnabled = false;

            this.Visibility = Visibility.Visible;
            SwivelTransition st = new SwivelTransition();
            st.Mode = SwivelTransitionMode.FullScreenIn;
            var it = st.GetTransition(this);
            it.Completed += new EventHandler(it_Completed);
            it.Begin();
            ShowItemsStoryBoard.Begin();
        }

        void it_Completed(object sender, EventArgs e)
        {
            
            this.IsEnabled = true;
           
        }
        public void Hide()
        {
            SlideTransition st = new SlideTransition();
            st.Mode = SlideTransitionMode.SlideDownFadeOut;
            var itClose = st.GetTransition(this);
            itClose.Completed += new EventHandler(itClose_Completed);
            itClose.Begin();
            //this.RenderTransform = new ScaleTransform()
            //{
            //    CenterX = this.Width / 2,
            //    CenterY = this.Height / 2
            //};
            //Storyboard sb = new Storyboard();
            //double from = 1;
            //double to = 0.1;
            //DoubleAnimation anix = new DoubleAnimation();
            //DoubleAnimation aniy = new DoubleAnimation();
            //anix.From = aniy.From = from;
            //anix.To = aniy.To = to;
            //anix.Duration = aniy.Duration = TimeSpan.FromSeconds(0.3);
            //Storyboard.SetTarget(anix, this.RenderTransform);
            //Storyboard.SetTarget(aniy, this.RenderTransform);
            //Storyboard.SetTargetProperty(anix, new PropertyPath(ScaleTransform.ScaleXProperty));
            //Storyboard.SetTargetProperty(aniy, new PropertyPath(ScaleTransform.ScaleYProperty));
            //sb.Children.Add(anix);
            //sb.Children.Add(aniy);
            //sb.Completed += new EventHandler(sb2_Completed);
            //sb.Begin();

            this.IsEnabled = false;

            this.Visibility = Visibility.Visible;
        }

        void itClose_Completed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;  
        }
        void sb_Completed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            this.RenderTransform = null;
        }
        void sb2_Completed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            this.RenderTransform = null;
            this.Visibility = Visibility.Collapsed;
        }
    }
}
