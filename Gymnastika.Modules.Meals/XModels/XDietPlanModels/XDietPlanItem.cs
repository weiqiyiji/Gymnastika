using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gymnastika.Modules.Meals.XModels.XDietPlanModels
{
    public class XDietPlanItem
    {
        [XmlAttribute("food-id")]
        public string FoodId { get; set; }

        [XmlAttribute("food-name")]
        public string FoodName { get; set; }

        [XmlAttribute("amount")]
        public decimal Amount { get; set; }
    }
}
