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
namespace Gymnastika.Phone.Controls
{
    public partial class AllSchdule : PhoneApplicationPage
    {
        GestureListener gestureListener;

        public ObservableCollection<ScheduleItem> SchduleItems { get; private set; }

        public AllSchdule()
        {
            InitializeComponent();
            gestureListener = GestureService.GetGestureListener(AllSchdulePanel);
            gestureListener.Hold += new EventHandler<Microsoft.Phone.Controls.GestureEventArgs>(gestureListener_Hold);
            gestureListener.DoubleTap += new EventHandler<GestureEventArgs>(gestureListener_DoubleTap);
            SchduleItems = new ObservableCollection<ScheduleItem>();
            SchduleItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(SchduleItems_CollectionChanged);


        }
        void SchduleItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (ScheduleItem item in e.NewItems)
                    {
                        AllSchdulePanel.Children.Add(new SchduleListItem(AllSchdulePanel.Children.Count, item));
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    List<SchduleListItem> listItems = new List<SchduleListItem>();
                    foreach (ScheduleItem item in e.OldItems)
                    {
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

        void gestureListener_DoubleTap(object sender, GestureEventArgs e)
        {
            OpenMenu();
        }
        void OpenMenu()
        {
            //if (AllSchduleList.SelectedItem == null)
            //    return;
            //ScheduleItem item = AllSchduleList.SelectedItem as ScheduleItem;
            //PopupMenu.PopupMenu menu = new PopupMenu.PopupMenu(AllSchduleList);
            //menu.Title = item.Name;
            //menu.Items.Add(new PopupMenu.MenuItem() { Text = "标记为完成" });
            //menu.Items.Add(new PopupMenu.MenuItem() { Text = "推迟" });
            //menu.Items.Add(new PopupMenu.MenuItem() { Text = "放弃" });
            //menu.Items.Add(new PopupMenu.MenuItem() { Text = "详细信息" });
            //menu.MenuClick += new PopupMenu.PopupMenu.MenuClickHandler(menu_MenuClick);
            //menu.Open();
        }
        void gestureListener_Hold(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            OpenMenu();
        }

        void menu_MenuClick(object sender, int MenuID, PopupMenu.MenuItem Menu)
        {

        }

    }
}