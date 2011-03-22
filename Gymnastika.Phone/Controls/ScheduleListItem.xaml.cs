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
using Gymnastika.Phone.Common;
namespace Gymnastika.Phone.Controls
{
    public partial class SchduleListItem : UserControl
    {
        private ScheduleItem m_Item;
        private int m_Index;
        public int Index { get { return m_Index; } set { m_Index = value; textBlock1.Text = (value+1).ToString()+"."; } }
        public SchduleListItem()
        {
            InitializeComponent();
        }
        public SchduleListItem(int Index,ScheduleItem Item)
            :this()
        {
            this.Index = Index;
            m_Item = Item;
            textBlock2.Text = string.Format("{0}-{1}", Item.Time.ToString("HH:mm:ss"), Item.Name);
            this.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(SchduleListItem_ManipulationStarted);
            this.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(SchduleListItem_ManipulationCompleted);
        }

        void SchduleListItem_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            this.LayoutRoot.Background = new SolidColorBrush(Colors.Black);
        }

        void SchduleListItem_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            this.LayoutRoot.Background = new SolidColorBrush(Colors.Gray);
       
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
