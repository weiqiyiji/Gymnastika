using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gymnastika.Modules.Sports.Models;
using System.Xml;
using System.IO;

namespace Gymnastika.Modules.Sports.DataImport.Sources
{
    public class XmlCategorySource : IDataSource<SportsCategory>
    {
        readonly string _xmlFilePath;
        const string SaveDir = "/Data/Sports/";

        public XmlCategorySource(string xmlFilePath)
        {
            _xmlFilePath = GetAbsolutePath(xmlFilePath);
        }

        public string GetAbsolutePath(string relativePath)
        {
            if (relativePath.Contains(":"))
                return relativePath;
            string currentDir =  Directory.GetCurrentDirectory();
            string absPath = Path.Combine(currentDir, relativePath);
            return absPath;
        }

        XmlDocument LoadXmlFile()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_xmlFilePath);
            return doc;
        }

        #region IDataSource<SportsCategory> Members

        public bool CanGetData()
        {
            try { LoadXmlFile(); }
            catch { return false; }
            return true;
        }

        public IEnumerable<SportsCategory> GetData()
        {
            XmlDocument doc = LoadXmlFile();
            IList<Sport> sports = new List<Sport>();
            var sportNodes = doc.FirstChild.ChildNodes;

            foreach (XmlNode node in sportNodes)
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
                        Sports = sports,
                        ImageUri = SaveDir + "Category.jpg"
                    }
                };
        }

        #endregion
    }

}
