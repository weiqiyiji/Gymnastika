using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gymnastika.Modules.Meals.XModels.XFoodLibraryModels
{
    public class XFood
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("Image")]
        public string LargeImageUri { get; set; }

        [XmlAttribute("Thumb")]
        public string SmallImageUri { get; set; }

        [XmlElement("Ingredient")]
        public XNutritionElement[] NutritionElements { get; set; }
    }
}
