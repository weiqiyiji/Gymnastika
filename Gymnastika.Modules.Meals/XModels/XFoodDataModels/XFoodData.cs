using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gymnastika.Modules.Meals.XModels.XFoodDataModels
{
    [XmlRoot("food-data")]
    public class XFoodData
    {
        [XmlArray("foods"), XmlArrayItem("food")]
        public XFood[] Foods { get; set; }
    }
}
