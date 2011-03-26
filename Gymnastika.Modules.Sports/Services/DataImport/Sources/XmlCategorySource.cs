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
        const string SaveDir = "/Data/Sport/";

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
            try
            {
                doc.Load(_xmlFilePath);
            }
            catch (Exception)
            {
                
            }
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

            IList<SportsCategory> categories = new List<SportsCategory>();
            foreach (XmlNode node in doc.FirstChild.ChildNodes)
            {
                SportsCategory category = new SportsCategory() { Sports = new List<Sport>() };
                categories.Add(category);
                category.Name = node.Attributes[0].InnerText;
                foreach (XmlNode spt in node.ChildNodes)
                {
                    var sport = new Sport();
                    var nodes = spt.ChildNodes;
                    sport.ImageUri = SaveDir + nodes.Item(1).InnerText;
                    sport.Name = nodes.Item(2).InnerText;
                    sport.Calories = Int32.Parse(nodes.Item(3).InnerText);
                    sport.Minutes = Int32.Parse(nodes.Item(4).InnerText);
                    sport.IntroductionUri = nodes.Item(5).InnerText;
                    category.Sports.Add(sport);
                }
            }
            return categories;
        }

        #endregion
    }

}
