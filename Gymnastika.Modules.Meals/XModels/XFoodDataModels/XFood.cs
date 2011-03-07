using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gymnastika.Modules.Meals.XModels.XFoodDataModels
{
    public class XFood
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("id")]
        public string ImageUri { get; set; }

        [XmlArray("categories"), XmlArrayItem("category")]
        public XCategory[] Categories { get; set; }

        [XmlArray("heat-content"), XmlArrayItem("content")]
        public XNutritionalElement[] NutritionalElements { get; set; }

        [XmlArray("introductions"), XmlArrayItem("introduction")]
        public XIntroduction[] Introductions { get; set; }
    }
}
