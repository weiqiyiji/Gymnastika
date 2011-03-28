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

namespace Gymnastika.Phone.Controls
{
    public partial class TimeSpanSelector : UserControl
    {
        GestureListener gestureListener;
        private int m_Minutes = 0;
        public int Minutes 
        { 
            get { return m_Minutes; }
            set { 
                m_Minutes = value; 
                if (m_Minutes < 0)
                    txtKind.Text = "提前"; 
                else 
                    txtKind.Text = "推迟";
                txtMinutes.Text = Math.Abs(value).ToString();
            } 
        }
        public TimeSpan Value
        {
            get { return TimeSpan.FromMinutes(Minutes); }
            set { Minutes = (int)value.TotalMinutes; }
        }
        public TimeSpanSelector()
        {
            InitializeComponent();
            gestureListener = GestureService.GetGestureListener(this);
            gestureListener.DragStarted += new EventHandler<DragStartedGestureEventArgs>(gestureListener_DragStarted);
            gestureListener.DragDelta += new EventHandler<DragDeltaGestureEventArgs>(gestureListener_DragDelta);
            gestureListener.DragCompleted += new EventHandler<DragCompletedGestureEventArgs>(gestureListener_DragCompleted);
        }
        private double CurrentValue;
        void gestureListener_DragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            
        }

        void gestureListener_DragStarted(object sender, DragStartedGestureEventArgs e)
        {
            CurrentValue = this.Minutes;
        }

        void gestureListener_DragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            double newValue = CurrentValue - e.VerticalChange / 8;
            CurrentValue = newValue;
            Minutes = (int)CurrentValue;
        }

    }
}
