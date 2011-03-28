using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace Gymnastika.Modules.Sports.Communication.Helper
{
    public static class ContractObjectSerializer
    {
        public static string Serialize(object o)
        {
            DataContractSerializer sr = new DataContractSerializer(o.GetType());
            using (MemoryStream strm = new MemoryStream())
            {
                sr.WriteObject(strm, o);
                strm.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(strm))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static T Deserialize<T>(string xml)
        {
            DataContractSerializer sr = new DataContractSerializer(typeof(T));

            using (MemoryStream strm = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                return (T)sr.ReadObject(strm);
            }

        }
    }
}
