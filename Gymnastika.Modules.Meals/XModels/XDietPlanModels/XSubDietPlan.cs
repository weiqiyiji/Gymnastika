using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gymnastika.Modules.Meals.XModels.XDietPlanModels
{
    public class XSubDietPlan
    {
        [XmlArray("foods"), XmlArrayItem("food")]
        public XDietPlanItem[] DietPlanItems { get; set; }
    }
}
