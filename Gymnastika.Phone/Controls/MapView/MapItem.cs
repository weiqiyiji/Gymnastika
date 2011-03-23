using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gymnastika.Phone.Controls.MapView
{
    public class MapItem : ContentControl
    {
        public virtual void PrepareToShow() { }
        public virtual void PrepareToHide() { }
        public virtual void StartShow(TimeSpan TotalTime) { }
        public virtual void StartHide(TimeSpan TotalTime) { }
        public MapItem()
        {
        }
        public void ChangeSize(double width, double height)
        {
            this.Width = width;
            this.Height = height;
            if (this.Content != null)
            {
                (this.Content as FrameworkElement).Width = width;
                (this.Content as FrameworkElement).Height = height;
            }
        }
        public new FrameworkElement Content
        {
            get { return this.Content as FrameworkElement; }
            set { base.Content = value; }
        }
        public MapItem(FrameworkElement Content)
            : this()
        {

            this.Content = Content;
            Content.Margin = new Thickness(0, 0, 0, 0);
            Content.Width = this.Width;
        }
    }
}
