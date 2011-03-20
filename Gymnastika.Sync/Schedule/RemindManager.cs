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
                IRepository<Connection> connectionRepository = ServiceLocator.Current.GetInstance<IRepository<Connection>>();

                foreach (ScheduleItem item in e.NewItems)
                {
                    TimeSpan countDown = item.StartTime - now;
                    Connection connection = connectionRepository.Get(x => x.Id == item.ConnectionId);
                    var source = connection.Source.NetworkAdapters.Count();
                    var target = connection.Target.Uri;

                    Timer timer = new Timer();
                    TimerMetadata metadata = new TimerMetadata(timer, item);
                    metadata.Connection = connection;
                    _metadataCollection.Add(metadata);

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
            timer.Stop();
            timer.Elapsed -= OnTimerElapsed;

            byte[] payload = PreparePayload(metadata);
            var utility = new NotificationSenderUtility();
            utility.SendRawNotification(
                new List<Uri>() { new Uri(metadata.Connection.Target.Uri) }, payload, null);

            _metadataCollection.Remove(metadata);
        }

        private byte[] PreparePayload(TimerMetadata metadata)
        {
            MemoryStream stream = new MemoryStream();

            XDocument doc = new XDocument(
                new XElement("plan",
                    new XElement("connection",
                        new XAttribute("id", metadata.Connection.Id)),
                    new XElement("data", metadata.ScheduleItem.Message)));

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