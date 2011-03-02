using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gymnastika.Modules.Meals.XModels.XCategoryDataModels
{
    [XmlRoot("category-data")]
    public class XCategoryData
    {
        [XmlArray("categories"), XmlArrayItem("category")]
        public XCategory[] Categories { get; set; }
    }
}
