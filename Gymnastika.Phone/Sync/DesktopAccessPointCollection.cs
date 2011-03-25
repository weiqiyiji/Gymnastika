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
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
namespace Gymnastika.Phone.Sync
{

    public class DesktopAccessPointCollection : IEnumerable<DesktopAccessPoint>
    {
        private List<DesktopAccessPoint> accessPoints = new List<DesktopAccessPoint>();
        public void Add(DesktopAccessPoint Point)
        {
            accessPoints.Add(Point);
        }
        public void Add(Uri PointUri)
        {
            accessPoints.Add(new DesktopAccessPoint() { Uri = PointUri });
        }
        public bool Remove(DesktopAccessPoint Point)
        {
            return accessPoints.Remove(Point);
        }
        public void RemoveAt(int Index)
        {
            accessPoints.RemoveAt(Index);
        }
        public void AddRange(IEnumerable<DesktopAccessPoint> range)
        {
            accessPoints.AddRange(range);
        }
        public void RemoveRange(int index,int count)
        {
            accessPoints.RemoveRange(index, count);
        }

        public IEnumerator<DesktopAccessPoint> GetEnumerator()
        {
            return accessPoints.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return accessPoints.GetEnumerator();
        }

        public void Serialize(Stream Target)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DesktopAccessPointCollection));
            xmlSerializer.Serialize(Target, this);
        }
        public static DesktopAccessPointCollection Deserialize(Stream source)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DesktopAccessPointCollection));
            return xmlSerializer.Deserialize(source) as DesktopAccessPointCollection;
        }
    }
}
