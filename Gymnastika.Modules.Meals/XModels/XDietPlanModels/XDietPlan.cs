using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gymnastika.Modules.Meals.XModels.XDietPlanModels
{
    public class XDietPlan
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("plan-type")]
        public bool PlanType { get; set; }

        [XmlArray("sub-diet-plan"), XmlArrayItem("sub-diet-plan")]
        public XSubDietPlan[] SubDietPlans { get; set; }
    }
}
