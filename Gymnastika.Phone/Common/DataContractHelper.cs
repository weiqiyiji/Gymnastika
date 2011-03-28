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
using System.IO;
using System.Runtime.Serialization;
using System.Text;
namespace Gymnastika.Phone.Common
{
    public class DataContractHelper
    {
        public static void Contract(Stream target,object o)
        {
            DataContractSerializer ser = new DataContractSerializer(o.GetType());
            ser.WriteObject(target, o);
        }
        public static T Decontract<T>(Stream source)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            return (T)ser.ReadObject(source);
        }
        public static string ContractString(object o)
        {
            DataContractSerializer ser = new DataContractSerializer(o.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                {
                    ser.WriteObject(ms, o);
                    ms.Seek(0, SeekOrigin.Begin);
                    return reader.ReadToEnd();
                }
            }
        }
        public static T Decontract<T>(string source)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            return (T)ser.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(source)));
        }
    }
}
