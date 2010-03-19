using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Timers;
using Gymnastika.Sync.Communication;
using System.IO;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Data;
using Gymnastika.Sync.Models;
using Gymnastika.Sync.Common;
using System.Xml;
using System.Text;
using System.Xml.Linq;

namespace Gymnastika.Sync.Schedule
{
    public class RemindManager
    {
        private ObservableCollection<ScheduleItem> _scheduleItems;
        private IList<TimerMetadata> _metadataCollection;

        public RemindManager()
        {
            _metadataCollection = new List<TimerMetadata>();
            _scheduleItems = new ObservableCollection<ScheduleItem>();
            _scheduleItems.CollectionChanged += OnScheduleItemsChanged;
        }

        private void OnScheduleItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                DateTime now = DateTime.Now;

                foreach (ScheduleItem item in e.NewItems)
                {
                    TimeSpan countDown = item.StartTime - now;

                    Timer timer = new Timer();
                    _metadataCollection.Add(new TimerMetadata(timer, item));
                    timer.Interval = countDown.TotalMilliseconds;
                    timer.Elapsed += OnTimerElapsed;
                    timer.Enabled = true;
                    timer.Start();
                }
            }
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Timer timer = (Timer)sender;
            TimerMetadata metadata = _metadataCollection.Single(x => x.Timer == timer);
            ScheduleItem scheduleItem = metadata.ScheduleItem;
            DesktopClient src = null;
            PhoneClient desc = null;

            byte[] payload = PreparePayload(scheduleItem, out src, out desc);
            var utility = new NotificationSenderUtility();
            utility.SendRawNotification(
                new List<Uri>() { new Uri(desc.Uri) }, payload, null);

            _scheduleItems.Remove(scheduleItem);
            _metadataCollection.Remove(metadata);
        }

        private byte[] PreparePayload(ScheduleItem scheduleItem, out DesktopClient src, out PhoneClient desc)
        {
            MemoryStream stream = new MemoryStream();
            IServiceLocator serviceLocator = ServiceLocator.Current;
            int connectionId = scheduleItem.ConnectionId;

            using (serviceLocator.GetInstance<IWorkEnvironment>().GetWorkContextScope())
            {
                IRepository<Connection> connectionRepository = serviceLocator.GetInstance<IRepository<Connection>>();
                Connection connection = connectionRepository.Get(x => x.Id == connectionId);

                src = connection.Source;
                desc = connection.Target;
            }

            XDocument doc = new XDocument(
                new XElement("plan", 
                    new XElement("connection",
                        new XAttribute("id", connectionId)),
                    new XElement("data", scheduleItem.Message)));

            doc.Save(stream);
            byte[] payload = stream.ToArray();
            stream.Close();

            return payload;
        }

        public void Add(ScheduleItem scheduleItem)
        {
            _scheduleItems.Add(scheduleItem);
        }
    }
}