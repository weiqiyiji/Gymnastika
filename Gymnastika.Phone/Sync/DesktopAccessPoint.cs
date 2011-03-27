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
        public string Token { get; set; }
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }
        public void ReadXml(System.Xml.XmlReader reader)
        {
          
            Token = reader.GetAttribute("token");
            m_uri = new Uri(reader.ReadElementContentAsString());
        }
        public void WriteXml(System.Xml.XmlWriter writer)
        {

            writer.WriteAttributeString("token", Token);
            writer.WriteString(Uri.ToString());
        }
        public Uri GetRelativeUri(string relativUri)
        {
            return new Uri(m_uri, relativUri);
        }
        public override string ToString()
        {
            return string.Format("{0};{1}", m_uri, Token);    
        }
    }
}
