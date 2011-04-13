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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace Gymnastika.Phone.Common
{
    public class SchduleListener
    {
        public class ScheduleArg : EventArgs
        {
            public ScheduleItem Item { get; set; }
            public ScheduleArg(object item)
            {
                this.Item = item as ScheduleItem;
            }
        }
        public class ScheduleStatusChangedArg : ScheduleArg
        {
            public ScheduleItemStatus OldStatus { get; set; }
            public ScheduleItemStatus NewStatus { get; set; }

            public ScheduleStatusChangedArg(object item, ScheduleItemStatus oldStatus, ScheduleItemStatus newStatus)
                : base(item)
            {
                this.OldStatus = oldStatus;
                this.NewStatus = newStatus;
            }
        }
        public event EventHandler<ScheduleStatusChangedArg> ScheduleStatusChagned;
        public event EventHandler<ScheduleArg> ScheduleBegin;
        public event EventHandler<ScheduleArg> ScheduleCompelted;
        public ObservableCollection<ScheduleItem> Children { get; private set; }
        Dictionary<ScheduleItem, bool> Alerted = new Dictionary<ScheduleItem, bool>();
        Timer CheckTimer;
        public SchduleListener()
        {
            Children = new ObservableCollection<ScheduleItem>();
            ChildStatusChanged = new ScheduleItem.StatusChangedHandler(Child_StatusChange);
            Children.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Children_CollectionChanged);
        }
        ScheduleItem.StatusChangedHandler ChildStatusChanged;
        void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    if(e.NewItems!=null)
                    foreach (ScheduleItem item in e.NewItems)
                    {
                        Alerted.Add(item, false);
                        item.StatusChange += ChildStatusChanged;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    if(e.OldItems!=null)
                    foreach (ScheduleItem item in e.OldItems)
                    {
                        Alerted.Remove(item);
                        item.StatusChange -= ChildStatusChanged;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    if(e.OldItems!=null)
                    foreach (ScheduleItem item in e.OldItems)
                    {
                        item.StatusChange -= ChildStatusChanged;
                    }
                    if(e.NewItems!=null)
                    foreach (ScheduleItem item in e.NewItems)
                    {
                        Alerted.Add(item, false);
                        item.StatusChange += ChildStatusChanged;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    if(e.OldItems!=null)
                    foreach (ScheduleItem item in e.OldItems)
                    {
                        Alerted.Remove(item);
                        item.StatusChange -= ChildStatusChanged;
                    }
                    Alerted.Clear();
                    break;
            }
        }

        void Child_StatusChange(object sender, ScheduleItemStatus OldStatus, ScheduleItemStatus NewStatus)
        {
            if (ScheduleStatusChagned != null)
                ScheduleStatusChagned(this, new ScheduleStatusChangedArg(sender, OldStatus, NewStatus));
            if (NewStatus == ScheduleItemStatus.Done)
                if (ScheduleCompelted != null)
                    ScheduleCompelted(this, new ScheduleArg(sender));
        }
        public void Start(System.Windows.Threading.Dispatcher dispatcher)
        {

            CheckTimer = new Timer(
                new TimerCallback((sender) =>
                {
                    dispatcher.BeginInvoke(new Action(() =>
                    {
                 
                        foreach (ScheduleItem item in Children)
                        {
                            double offset = (DateTime.Now - item.Time).TotalSeconds;
                            double over = offset - item.Duration.TimeSpan.TotalSeconds;
                            if (offset < 0) continue;
                            if (offset < 0.5)
                            {
                                if (ScheduleBegin != null && item.Status != ScheduleItemStatus.Active && item.Status != ScheduleItemStatus.Aborted)
                                {
                                    ScheduleBegin(this, new ScheduleArg(item));
                                }
                            }
                            if (over >= 0 && over < 0.5)
                            {
                                if (ScheduleCompelted != null && item.Status == ScheduleItemStatus.Active)
                                {
                                    ScheduleCompelted(this, new ScheduleArg(item));
                                }
                            }

                        }
                    }));
                }
                ));
            CheckTimer.Change(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.5));
        }
    }
}
