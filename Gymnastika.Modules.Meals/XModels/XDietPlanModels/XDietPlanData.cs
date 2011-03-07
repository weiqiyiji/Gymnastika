using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gymnastika.Modules.Meals.XModels.XDietPlanModels
{
    [XmlRoot("diet-plan-data")]
    public class XDietPlanData
    {
        [XmlArray("diet-plans"), XmlArrayItem("diet-plan")]
        public XDietPlan[] DietPlans { get; set; }
    }
}
