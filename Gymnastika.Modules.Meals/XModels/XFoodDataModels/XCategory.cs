using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gymnastika.Modules.Meals.XModels.XFoodDataModels
{
    public class XCategory
    {
        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
