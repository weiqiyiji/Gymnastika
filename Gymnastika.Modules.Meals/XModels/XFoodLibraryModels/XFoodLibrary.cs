using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gymnastika.Modules.Meals.XModels.XFoodLibraryModels
{
    [XmlRoot("FoodLibrary")]
    public class XFoodLibrary
    {
        [XmlElement("class")]
        public XClass[] Classes { get; set; }
    }
}
