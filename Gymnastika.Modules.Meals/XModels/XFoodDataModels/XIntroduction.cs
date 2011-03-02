using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gymnastika.Modules.Meals.XModels.XFoodDataModels
{
    public class XIntroduction
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("content")]
        public string Content { get; set; }
    }
}
