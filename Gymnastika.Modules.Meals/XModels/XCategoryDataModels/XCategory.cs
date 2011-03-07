using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gymnastika.Modules.Meals.XModels.XCategoryDataModels
{
    public class XCategory
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("image-uri")]
        public string ImageUri { get; set; }

        [XmlArray("sub-categories"), XmlArrayItem("sub-category")]
        public XSubCategory[] SubCategories { get; set; }
    }
}
