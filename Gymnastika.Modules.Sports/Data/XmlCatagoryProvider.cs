using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using System.Xml;
using System.IO;
using Gymnastika.Modules.Sports.Services;

namespace Gymnastika.Modules.Sports.Data
{
    public class XmlCatagoryProvider : ICategoriesProvider
    {
        const string SaveDir = "/Data/Sports/";
        public IEnumerable<SportsCategory> Fetch(Func<SportsCategory, bool> predicate)
        {
            IList<Sport> sports = new List<Sport>();
            XmlDocument doc = new XmlDocument();
            const string relativePath = SaveDir + "SportData.xml";
            string path = Directory.GetCurrentDirectory() + relativePath;
            doc.Load(path);
            foreach (XmlNode node in doc.FirstChild.ChildNodes)
            {
                Sport sport = new Sport();
                sport.Brief = "";

                var nodes = node.ChildNodes;
                sport.ImageUri = SaveDir + nodes.Item(1).InnerText;
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

        #region IProvider<SportsCategory> Members


        public IEnumerable<SportsCategory> Fetch(int startIndex, int number)
        {
            return Fetch((s) => true).Take(number).Skip(startIndex);
        }

        public void CreateOrUpdate(SportsCategory entity)
        {
            throw new NotImplementedException();
        }

        public void Create(SportsCategory entity)
        {
            throw new NotImplementedException();
        }

        public void Update(SportsCategory entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(SportsCategory entity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
