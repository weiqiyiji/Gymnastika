using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.XModels.XFoodDataModels;
using Gymnastika.Modules.Meals.XModels.XCategoryDataModels;
using Gymnastika.Modules.Meals.XModels.XDietPlanModels;
using System.IO;
using System.Xml.Serialization;

namespace Gymnastika.Modules.Meals.XDataHelpers
{
    public class XDataFileManager
    {
        private readonly string _foodDataFilePath;
        private readonly string _categoryDataFilePath;
        private readonly string _dietPlanDataFielPath;

        private XFoodData _xFoodData;
        private XCategoryData _xCategoryData;
        private XDietPlanData _xDietPlanData;

        public XDataFileManager()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string dataDirectory = currentDirectory + "\\Datas\\";

            _foodDataFilePath = dataDirectory + "foods.xml";
            _categoryDataFilePath = dataDirectory + "categories.xml";
            _dietPlanDataFielPath = dataDirectory + "dietplans.xml";
        }

        public XFoodData GetFoodData()
        {
            using (Stream stream = GetStream(_foodDataFilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(XFoodData));
                _xFoodData = (XFoodData)serializer.Deserialize(stream);
            }

            return _xFoodData;
        }

        public XCategoryData GetCategoryData()
        {
            using (Stream stream = GetStream(_categoryDataFilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(XCategoryData));
                _xCategoryData = (XCategoryData)serializer.Deserialize(stream);
            }

            return _xCategoryData;
        }

        public XDietPlanData GetDietPlanData()
        {
            using (Stream stream = GetStream(_dietPlanDataFielPath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(XDietPlanData));
                _xDietPlanData = (XDietPlanData)serializer.Deserialize(stream);
            }

            return _xDietPlanData;
        }

        private Stream GetStream(string filePath)
        {
            return File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
    }
}
