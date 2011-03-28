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
using Gymnastika.Phone.Common;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
namespace Gymnastika.Phone.Controls
{
    public partial class AllSchdule : PhoneApplicationPage
    {

        public ObservableCollection<ScheduleItem> SchduleItems { get; private set; }

        public AllSchdule()
        {
            InitializeComponent();
            //gestureListener = GestureService.GetGestureListener(AllSchdulePanel);
            //gestureListener.Hold += new EventHandler<Microsoft.Phone.Controls.GestureEventArgs>(gestureListener_Hold);
            //gestureListener.DoubleTap += new EventHandler<GestureEventArgs>(gestureListener_DoubleTap);

            SchduleItems = new ObservableCollection<ScheduleItem>();
            SchduleItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(SchduleItems_CollectionChanged);
            OpenMenuGesture = new EventHandler<GestureEventArgs>(OpenMenuGestureEntry);

        }
        private class SchduleComparision : IComparer<SchduleListItem>
        {
            public int Compare(SchduleListItem x, SchduleListItem y)
            {
                if (x.Schedule.Time > y.Schedule.Time)
                    return 1;
                else if (x.Schedule.Time < y.Schedule.Time)
                    return -1;
                else
                    return 0;
            }
        }

        public void Sort()
        {
            //gather
            List<SchduleListItem> orgin = new List<SchduleListItem>();
            foreach (UIElement el in AllSchdulePanel.Children)
            {
                if (el is SchduleListItem)
                {
                    orgin.Add(el as SchduleListItem);
                }
            }
            //sort
            orgin.Sort(new SchduleComparision());
            AllSchdulePanel.Children.Clear();
            for (int i = 0; i < orgin.Count; i++)
            {
                AllSchdulePanel.Children.Add(orgin[i]);
            }
        }
        void SchduleItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (ScheduleItem item in e.NewItems)
                    {
                        SchduleListItem listItem = new SchduleListItem(AllSchdulePanel.Children.Count, item);
                        GestureListener gl = GestureService.GetGestureListener(listItem);
                        gl.Hold += OpenMenuGesture;
                        AllSchdulePanel.Children.Add(listItem);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    List<SchduleListItem> listItems = new List<SchduleListItem>();
                    foreach (ScheduleItem item in e.OldItems)
                    {
                        GestureListener gl = GestureService.GetGestureListener(item);
                        gl.Hold -= OpenMenuGesture;
                        for (int i = 0; i < AllSchdulePanel.Children.Count; i++)
                            if (AllSchdulePanel.Children[i] is SchduleListItem)
                            {
                                if (((SchduleListItem)AllSchdulePanel.Children[i]).Equals(item))
                                {
                                    AllSchdulePanel.Children.RemoveAt(i);
                                    i--;
                                }
                            }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    AllSchdulePanel.Children.Clear();
                    break;
            }

        }
        private EventHandler<GestureEventArgs> OpenMenuGesture;

        void OpenMenuGestureEntry(object sender, GestureEventArgs e)
        {
            FrameworkElement fe = e.OriginalSource as FrameworkElement;
            while (fe != null)
            {
                if (fe is SchduleListItem)
                {
                    OpenMenu(fe as SchduleListItem);
                    break;
                }
                fe = fe.Parent as FrameworkElement;
            }

        }
        void OpenMenu(SchduleListItem Item)
        {
            if (Item.Schedule.Status == ScheduleItemStatus.Done)
                return;
            PopupMenu.PopupMenu menu = new PopupMenu.PopupMenu(Item);
            menu.Tag = Item;
            menu.Title = Item.Schedule.Name;
            menu.AddItem(new PopupMenu.MenuItem() { Text = "标记为完成" }).Click += new EventHandler<PopupMenu.MenuItem.MenuClickArg>((sender, e) =>
            { Item.Schedule.Status = ScheduleItemStatus.Done; });
            menu.AddItem(new PopupMenu.MenuItem() { Text = "推迟/提前" }).Click += new EventHandler<PopupMenu.MenuItem.MenuClickArg>((sender, e) =>
            {
                Popup popup = new Popup();
                Canvas overlay = new Canvas();
                FrameworkElement _root = App.Current.RootVisual as FrameworkElement;
                Size screenSize = new Size(_root.ActualWidth, _root.ActualHeight);
                WriteableBitmap bmp = new WriteableBitmap((int)screenSize.Width, (int)screenSize.Height);
                bmp.Render(_root, null);
                bmp.Invalidate();
                Image contentLayer = new Image() { Source = bmp };
                TimeSpanSelector selector = new TimeSpanSelector();
                contentLayer.MouseLeftButtonDown += new MouseButtonEventHandler((sender_, e_) =>
                {
                    popup.IsOpen = false;
                    TimeSpan span = selector.Value;
                    if (Item.Schedule.Time + span < DateTime.Now)
                    {
                        MessageBox.Show("不能将计划项提前到当前时间之前。");
                        return;
                    }
                    if (span.TotalMinutes != 0)
                    {
                        Item.Schedule.Delay(span);
                        Sort();
                    }
                }
                );
                overlay.Children.Add(contentLayer);
                overlay.Children.Add(selector);
                selector.SetValue(Canvas.LeftProperty, screenSize.Width / 2 - selector.Width / 2);
                selector.SetValue(Canvas.TopProperty, screenSize.Height / 2 - selector.Height / 2);
                popup.Child = overlay;
                popup.IsOpen = true;
            });
            menu.AddItem(new PopupMenu.MenuItem() { Text = "放弃" }).Click += new EventHandler<PopupMenu.MenuItem.MenuClickArg>((sender, e) =>
            { Item.Schedule.Status = ScheduleItemStatus.Aborted; });
            menu.MenuClick += new PopupMenu.PopupMenu.MenuClickHandler(menu_MenuClick);
            menu.Open();
        }
        void menu_MenuClick(object sender, int MenuID, PopupMenu.MenuItem Menu)
        {

        }

    }
}