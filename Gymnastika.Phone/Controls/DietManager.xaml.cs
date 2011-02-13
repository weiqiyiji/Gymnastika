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
    public partial class DietManager : UserControl
    {
        private class Group
        {
            public RadioButton Button { get; protected set; }
            public DietItem Item { get; protected set; }
            public double MaxHeight { get; set; }
            Storyboard opencloseStoryboard = new Storyboard();
            public Group(RadioButton Button, DietItem Item)
            {
                this.Button = Button;
                this.Item = Item;
                MaxHeight = 300;
            }
            private void ClearStoryBoard()
            {

                opencloseStoryboard.Stop();
                opencloseStoryboard.Children.Clear();
            }
            public void Open()
            {
                ClearStoryBoard();
                DoubleAnimation ani = new DoubleAnimation();
                ani.From = Item.Height;
                ani.To = MaxHeight;
                ani.Duration = TimeSpan.FromSeconds(0.3);
                Storyboard.SetTarget(ani, Item);
                Storyboard.SetTargetProperty(ani, new PropertyPath(DietItem.HeightProperty));
                opencloseStoryboard.Children.Add(ani);
                opencloseStoryboard.Begin();
            }
            public void Close()
            {
                ClearStoryBoard();
                DoubleAnimation ani = new DoubleAnimation();
                ani.From = Item.Height;
                ani.To = 0;
                ani.Duration = TimeSpan.FromSeconds(0.3);
                Storyboard.SetTarget(ani, Item);
                Storyboard.SetTargetProperty(ani, new PropertyPath(DietItem.HeightProperty));
                opencloseStoryboard.Children.Add(ani);
                opencloseStoryboard.Begin();
            }
        }
        Dictionary<RadioButton, Group> Groups = new Dictionary<RadioButton, Group>();
        public DietManager()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DietManager_Loaded);
        }

        void DietManager_Loaded(object sender, RoutedEventArgs e)
        {
            Groups.Add(rb1,new Group(rb1, di1));
            Groups.Add(rb2,new Group(rb2, di2));
            Groups.Add(rb3,new Group(rb3, di3));
            SwitchTo(rb1);
            rb1.Checked += new RoutedEventHandler(Switch);
            rb2.Checked += new RoutedEventHandler(Switch);
            rb3.Checked += new RoutedEventHandler(Switch);
        }

        void Switch(object sender, RoutedEventArgs e)
        {
            SwitchTo((RadioButton)sender);
        }
        private void SwitchTo(RadioButton index)
        {
            foreach (KeyValuePair<RadioButton, Group> g in Groups)
            {
                if (g.Key.Equals(index))
                    g.Value.Open();
                else
                    g.Value.Close();
            }
        }
    }
}
