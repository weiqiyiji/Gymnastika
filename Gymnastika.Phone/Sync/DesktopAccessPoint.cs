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
namespace Gymnastika.Phone.Sync
{
    public class DesktopAccessPoint : IXmlSerializable
    {
        private Uri m_uri;
        public Uri Uri { get { return m_uri; } set { this.m_uri = value; } }
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }
        public void ReadXml(System.Xml.XmlReader reader)
        {
            m_uri = new Uri(reader.ReadContentAsString());
        }
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteString(m_uri.ToString());
        }
    }
}
