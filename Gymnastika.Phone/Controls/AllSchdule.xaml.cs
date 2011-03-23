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

        ObservableCollection<ScheduleItem> SchduleItems = new ObservableCollection<ScheduleItem>();
        public AllSchdule()
        {
            InitializeComponent();
            gestureListener=GestureService.GetGestureListener(AllSchduleList);
            gestureListener.Hold += new EventHandler<Microsoft.Phone.Controls.GestureEventArgs>(gestureListener_Hold);
            gestureListener.DoubleTap += new EventHandler<GestureEventArgs>(gestureListener_DoubleTap);
            for (int i = 0; i < 10; i++)    
            {
                SchduleItems.Add(new ScheduleItem()
                {
                   // Icon = new BitmapImage(new Uri("/Gymnastika.Phone;component/Images/appbar.check.rest.png", UriKind.Relative)),
                    Name = "test" + i.ToString(),
                    Status = ScheduleItemStatus.Active,
                    Time = DateTime.Now.AddMinutes(i * i)
                });
            }
            AllSchduleList.ItemsSource = SchduleItems;

        }

        void gestureListener_DoubleTap(object sender, GestureEventArgs e)
        {
            OpenMenu();
        }
        void OpenMenu()
        {
            if (AllSchduleList.SelectedItem == null)
                return;
            ScheduleItem item = AllSchduleList.SelectedItem as ScheduleItem;
            PopupMenu.PopupMenu menu = new PopupMenu.PopupMenu(AllSchduleList);
            menu.Title = item.Name;
            menu.Items.Add(new PopupMenu.MenuItem() { Text = "标记为完成" });
            menu.Items.Add(new PopupMenu.MenuItem() { Text = "推迟" });
            menu.Items.Add(new PopupMenu.MenuItem() { Text = "放弃" });
            menu.Items.Add(new PopupMenu.MenuItem() { Text = "详细信息" });
            menu.MenuClick += new PopupMenu.PopupMenu.MenuClickHandler(menu_MenuClick);
            menu.Open();
        }
        void gestureListener_Hold(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            OpenMenu();
        }

        void menu_MenuClick(object sender, int MenuID, PopupMenu.MenuItem Menu)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ScheduleItem item = AllSchduleList.SelectedItem as ScheduleItem;
            if (item != null)
            {
                item.Time = item.Time.AddMinutes(5);
            }
        }
    }
}