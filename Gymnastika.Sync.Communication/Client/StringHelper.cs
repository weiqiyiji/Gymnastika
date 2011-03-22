using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Gymnastika.Sync.Communication.Client
{
    public class StringHelper
    {
        public static string GetPureString(string input)
        {
            return XElement.Parse(input).Value;
        }
    }
}
