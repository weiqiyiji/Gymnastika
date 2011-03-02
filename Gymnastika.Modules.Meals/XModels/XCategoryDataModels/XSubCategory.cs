using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gymnastika.Modules.Meals.XModels.XCategoryDataModels
{
    public class XSubCategory
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
