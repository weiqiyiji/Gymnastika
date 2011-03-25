using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.XModels.XFoodDataModels;
using Gymnastika.Modules.Meals.XModels.XCategoryDataModels;
using Gymnastika.Modules.Meals.XModels.XDietPlanModels;
using System.IO;
using System.Xml.Serialization;
using Gymnastika.Modules.Meals.XModels.XFoodLibraryModels;

namespace Gymnastika.Modules.Meals.XDataHelpers
{
    public class XDataFileManager
    {
        private readonly string _foodDataFilePath;
        private readonly string _categoryDataFilePath;
        private readonly string _dietPlanDataFielPath;
        private readonly string _foodLibraryFilePath;

        private XFoodData _xFoodData;
        private XCategoryData _xCategoryData;
        private XDietPlanData _xDietPlanData;
        private XFoodLibrary _xFoodLibrary;

        public XDataFileManager()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string dataDirectory = currentDirectory + "\\Datas\\";

            //_foodDataFilePath = dataDirectory + "foods.xml";
            _categoryDataFilePath = dataDirectory + "categories.xml";
            _dietPlanDataFielPath = dataDirectory + "dietplans.xml";

            _foodLibraryFilePath = dataDirectory + "foods.xml";
        }

        public XFoodLibrary GetFoodLibrary()
        {
            using (Stream stream = GetStream(_foodLibraryFilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(XFoodLibrary));
                _xFoodLibrary = (XFoodLibrary)serializer.Deserialize(stream);
            }

            return _xFoodLibrary;
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
