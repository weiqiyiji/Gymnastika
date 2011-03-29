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
        private readonly IList<TimerMetadata> _metadataCollection;
        private readonly IRepository<Gymnastika.Sync.Models.ScheduleItem> _scheduleRepository;
        private readonly IRepository<Connection> _connectionRepository;

        public RemindManager(
            IRepository<Connection> connectionRepository,
            IRepository<Gymnastika.Sync.Models.ScheduleItem> scheduleRepository)
        {
            _connectionRepository = connectionRepository;
            _scheduleRepository = scheduleRepository;
            _metadataCollection = new List<TimerMetadata>();
        }

        private void SetTimer(Models.ScheduleItem item, DateTime startTime)
        {
            DateTime now = DateTime.Now;

            TimeSpan countDown = startTime - now;

            Connection connection = _connectionRepository.Get(x => x.Id == item.ConnectionId);

            Timer timer = new Timer();
            timer.Interval = countDown.TotalMilliseconds;
            timer.Elapsed += OnTimerElapsed;
            timer.Enabled = true;

            TimerMetadata metadata = new TimerMetadata
            {
                Timer = timer,
                Schedule = item,
                TargetUri = connection.Target.Uri
            };

            _metadataCollection.Add(metadata);

            timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Timer timer = (Timer)sender;
            TimerMetadata metadata = _metadataCollection.Single(x => x.Timer == timer);
            timer.Stop();
            timer.Elapsed -= OnTimerElapsed;

            try
            {
                byte[] payload = PreparePayload(metadata.Schedule);
                var utility = new NotificationSenderUtility();
                utility.SendRawNotification(
                    new List<Uri>() { new Uri(metadata.TargetUri) }, payload, null);
            }
            catch(Exception) {}
            finally
            {
                _metadataCollection.Remove(metadata);
            }
        }

        private byte[] PreparePayload(Models.ScheduleItem schedule)
        {
            MemoryStream stream = new MemoryStream();

            XDocument doc = new XDocument(
                new XElement("schedule",
                    new XAttribute("id", schedule.Id),
                    new XElement("connection",
                        new XAttribute("id", schedule.ConnectionId)),
                    new XElement("user",
                        new XAttribute("id", schedule.UserId)),
                    new XElement("data", XElement.Parse(schedule.Message))));

            doc.Save(stream);
            byte[] payload = stream.ToArray();
            stream.Close();

            return payload;
        }

        public void Add(Gymnastika.Sync.Models.ScheduleItem scheduleItem)
        {
            SetTimer(scheduleItem, scheduleItem.StartTime);
        }
    }
}