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
using System.Threading;

namespace Gymnastika.Phone.Network
{
    public class LongAlive
    {
        Timer timer;
        public delegate void TickCountChangedHandler(object sender, int TickCount);
        public event TickCountChangedHandler TickCountChanged;
        public int TickCount { get;private set; }
        public LongAlive()
        {
            TickCount = 0;
            timer = new Timer(new TimerCallback((sender) =>
                {
                    if (TickCount == int.MaxValue)
                        TickCount = 0;
                    else
                    TickCount++;
                    if (TickCountChanged != null)
                        TickCountChanged(this, TickCount);
                    
                }));
            timer.Change(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }
    }
}
