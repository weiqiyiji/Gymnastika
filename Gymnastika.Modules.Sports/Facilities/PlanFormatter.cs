using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using System.Xml;

namespace Gymnastika.Modules.Sports.Facilities
{
    public class PlanFormatter
    {
            public static string Format(SportsPlan plan)
            {

                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Plan/>");
                XmlNode xPlan = doc.FirstChild;

                //Date
                XmlAttribute year = doc.CreateAttribute("Year");
                year.Value = plan.Year.ToString();
                xPlan.Attributes.Append(year);

                XmlAttribute month = doc.CreateAttribute("Month");
                month.Value = plan.Month.ToString();
                xPlan.Attributes.Append(month);

                XmlAttribute day = doc.CreateAttribute("Day");
                day.Value = plan.Day.ToString();
                xPlan.Attributes.Append(day);

                //Items
                StringBuilder builder = new StringBuilder();
                foreach (var item in plan.SportsPlanItems)
                {
                    string xItem = Format(item);
                    builder.Append(xItem);

                }

                xPlan.InnerXml = builder.ToString();
                return doc.InnerXml;
            }

            public static string Format(SportsPlanItem item)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Item/>");
                XmlNode xItem = doc.FirstChild;

                XmlAttribute hour = doc.CreateAttribute("Hour");
                hour.Value = item.Hour.ToString();
                xItem.Attributes.Append(hour);

                XmlAttribute minute = doc.CreateAttribute("Minute");
                minute.Value = item.Minute.ToString();
                xItem.Attributes.Append(minute);

                XmlAttribute duration = doc.CreateAttribute("Duration");
                duration.Value = item.Duration.ToString();
                xItem.Attributes.Append(duration);

                xItem.InnerXml = Format(item.Sport);
                return doc.InnerXml;
            }

            public static string Format(Sport sport)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Sport/>");
                XmlNode xSport = doc.FirstChild;

                XmlAttribute name = doc.CreateAttribute("Name");
                name.Value = sport.Name;
                xSport.Attributes.Append(name);

                XmlAttribute minutes = doc.CreateAttribute("Minutes");
                minutes.Value = sport.Minutes.ToString();
                xSport.Attributes.Append(minutes);

                XmlAttribute calories = doc.CreateAttribute("Calories");
                calories.Value = sport.Calories.ToString();
                xSport.Attributes.Append(calories);

                return doc.InnerXml;
            }
    }
}
