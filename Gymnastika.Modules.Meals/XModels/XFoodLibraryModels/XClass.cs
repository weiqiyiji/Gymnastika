using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gymnastika.Modules.Meals.XModels.XFoodLibraryModels
{
    public class XClass
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("food")]
        public XFood[] Foods { get; set; }
    }
}
