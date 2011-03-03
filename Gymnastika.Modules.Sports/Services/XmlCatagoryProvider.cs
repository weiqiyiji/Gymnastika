using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using System.Xml;
using System.IO;

namespace Gymnastika.Modules.Sports.Services
{
    public class XmlCatagoryProvider : ICategoriesProvider
    {
        #region ICategoriesProvider Members

        public IEnumerable<Models.SportsCategory> Fetch(Func<Models.SportsCategory, bool> predicate)
        {
            IList<Sport> sports = new List<Sport>();
            XmlDocument doc = new XmlDocument();
            string dir = Directory.GetCurrentDirectory();
            const string relativePath = @"\Data\SportData.xml";
            string path = dir + relativePath;
            doc.Load(path);
            foreach (XmlNode node in doc.FirstChild.ChildNodes)
            {
                Sport sport = new Sport();
                sport.Brief = "";

                var nodes = node.ChildNodes;
                sport.ImageUri = "Data/" + nodes.Item(1).InnerText;
                sport.Name = nodes.Item(2).InnerText;
                sport.Calories = Int32.Parse(nodes.Item(3).InnerText);
                sport.Minutes = Int32.Parse(nodes.Item(4).InnerText);
                sport.IntroductionUri = nodes.Item(5).InnerText;
                sports.Add(sport);
            }
            return new List<SportsCategory>()
                {
                    new SportsCategory()
                    {
                        Name = "默认分类",
                        Note = "来自Yaotiao",
                        Sports = sports
                    }
                };
        }

        #endregion

    }
}
