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

namespace Gymnastika.Phone.Controls.MapView
{
    public partial class MapContainer : UserControl
    {
        private enum MapItemSwitchAction
        {
            ToSummary = 1,
            ToDetails = 2,
            Showing = 4,
            Hiding = 6

        }
        public enum MapHotItem
        {
            Summary = 16,
            Details = 32
        }
        public MapItem Summary { get; set; }
        public MapItem Details { get; set; }
        public Point DefaultLocation { get; set; }
        public Size DefualtSize { get; set; }
        public MapHotItem HotItem { get; protected set; }
        private MapItemSwitchAction SwitchAction;
        private MapItemSwitchAction SwitchActionStep;
        private PlaneProjection MyProjection
        {
            get { return this.Projection as PlaneProjection; }
        }
        public MapContainer()
        {
            InitializeComponent();
            this.SizeChanged += new SizeChangedEventHandler(MapContainer_SizeChanged);
            this.Projection = new PlaneProjection();
            Storyboard.SetTargetProperty(SwitchAnimation, new PropertyPath(PlaneProjection.RotationYProperty));
            Storyboard.SetTarget(SwitchAnimation, this.Projection);
            SwitchAnimation.Completed += new EventHandler(SwitchAnimation_Completed);
            SwitchStoryBoard.Children.Add(SwitchAnimation);
        }
        public void ResetItemsSize()
        {
            if (Summary != null)
                Summary.ChangeSize(this.Width, this.Height);
            if (Details != null)
                Summary.ChangeSize(this.Width, this.Height);
        }
        void MapContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResetItemsSize();
        }
        public void SetHotItem(MapHotItem Item)
        {
            if (Item == MapHotItem.Details)
                this.Content = Details; 
            else
                this.Content = Summary;
            this.HotItem = Item;
        }
        void SwitchAnimation_Completed(object sender, EventArgs e)
        {
            if (this.SwitchActionStep == MapItemSwitchAction.Hiding)
            {
                if (this.HotItem == MapHotItem.Details)
                    this.Content = Summary;
                else
                    this.Content = Details;
                AnimateShow();

            }
            else
            {
                if (this.SwitchAction == MapItemSwitchAction.ToDetails)
                    this.HotItem = MapHotItem.Details;
                else
                    this.HotItem = MapHotItem.Summary;
            }
        }
        Storyboard SizeBoard = new Storyboard();
        DoubleAnimation WidthAnimation = new DoubleAnimation();
        Storyboard SwitchStoryBoard = new Storyboard();
        DoubleAnimation SwitchAnimation = new DoubleAnimation();
        private void AnimateShow()
        {
            SwitchStoryBoard.Stop();
            SwitchAnimation.From = -90;
            SwitchAnimation.To = 0;
            TimeSpan AnimateTime = TimeSpan.FromSeconds(0.9);
            SwitchAnimation.Duration = AnimateTime;
            if (this.SwitchAction == MapItemSwitchAction.ToSummary)
                Summary.StartShow(AnimateTime);
            else
                Details.StartShow(AnimateTime);
            this.SwitchActionStep = MapItemSwitchAction.Showing;
            SwitchStoryBoard.Begin();
        }
        private void AnimateHide()
        {
            SwitchStoryBoard.Stop();
            SwitchAnimation.From = 0;
            SwitchAnimation.To = 90;
            TimeSpan AnimateTime = TimeSpan.FromSeconds(.5);
            SwitchAnimation.Duration = AnimateTime;
            if (this.SwitchAction == MapItemSwitchAction.ToSummary)
                Details.StartHide(AnimateTime);
            else
                Summary.StartHide(AnimateTime);
            this.SwitchActionStep = MapItemSwitchAction.Hiding;
            SwitchStoryBoard.Begin();
        }
        public void ShowSummary()
        {
            Summary.PrepareToShow();
            Details.PrepareToHide();
            this.SwitchAction = MapItemSwitchAction.ToSummary;
            AnimateHide();

        }
        public void ShowDetais()
        {
            Summary.PrepareToHide();
            Details.PrepareToShow();
            this.SwitchAction = MapItemSwitchAction.ToDetails;
            AnimateHide();

        }
        public void Switch()
        {
            if (this.HotItem == MapHotItem.Summary)
                ShowDetais();
            else
                ShowSummary();
        }
    }
}
