using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.XModels.XCategoryDataModels;
using Gymnastika.Modules.Meals.XModels.XFoodDataModels;
using System.IO;
using System.Collections.ObjectModel;
using Gymnastika.Modules.Meals.Models;
using Gymnastika.Modules.Meals.XModels.XDietPlanModels;
using Gymnastika.Modules.Meals.Services;
using Gymnastika.Data;
using Gymnastika.Modules.Meals.XModels.XFoodLibraryModels;

namespace Gymnastika.Modules.Meals.XDataHelpers
{
    public class XDataRepository
    {
        private readonly IFoodService _foodService;
        private readonly IWorkEnvironment _workEnvironment;

        private XDataFileManager _dataFileManager;

        private XCategoryData _xCategoryData;
        private XFoodData _xFoodData;
        private XDietPlanData _xDietPlanData;
        private XFoodLibrary _xFoodLibrary;

        private string _categoryImagesDirectory;
        private string _foodImagesDirectory;

        public XDataRepository(IFoodService foodService, IWorkEnvironment workEnvironment)
        {
            _foodService = foodService;
            _workEnvironment = workEnvironment;
            _dataFileManager = new XDataFileManager();

            //_xCategoryData = _dataFileManager.GetCategoryData();
            //_xFoodData = _dataFileManager.GetFoodData();
            _xDietPlanData = _dataFileManager.GetDietPlanData();

            _xFoodLibrary = _dataFileManager.GetFoodLibrary();

            string currentDirectory = Directory.GetCurrentDirectory();
            string imagesDirectory = currentDirectory + "\\Data\\Food\\";
            _foodImagesDirectory = imagesDirectory + "Images\\";
        }

        //private bool _isStored;

        //public bool IsStored
        //{
        //    get
        //    {
        //        using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
        //        {
        //            int foodCount = _foodService.FoodProvider.GetAll().Count();
        //            _isStored = (foodCount == 8226);
        //            //_isStored = (_foodService.CategoryProvider.GetAll().Count() != 0);
        //        }

        //        return _isStored;
        //    }
        //}

        public void ExtractDatas()
        {
            ExtractFoodLibrary();
            //ExtractCategoryData();
            //ExtractFoodData();
            ExtractDietPlanData();
        }

        private void ExtractFoodLibrary()
        {
            Categories = new List<Category>();
            Foods = new List<Food>();
            NutritionalElements = new List<NutritionElement>();

            foreach (var xCategory in _xFoodLibrary.Classes)
            {
                Category category = new Category();
                category.Name = xCategory.Name;
                category.Foods = new List<Food>();
                foreach (var xFood in xCategory.Foods)
                {
                    Food food = new Food();
                    food.Name = xFood.Name;
                    food.ImageUri = _foodImagesDirectory + xFood.Id + ".png";
                    food.Calorie = xFood.NutritionElements[0].Value;
                    food.Category = category;
                    foreach (var xNutritionElement in xFood.NutritionElements)
                    {
                        NutritionElement nutritionElement = new NutritionElement();
                        nutritionElement.Name = xNutritionElement.Name;
                        nutritionElement.Value = xNutritionElement.Value;
                        nutritionElement.Food = food;
                        NutritionalElements.Add(nutritionElement);
                    }
                    Foods.Add(food);
                }
                Categories.Add(category);
            }
        }

        //private void ExtractCategoryData()
        //{
        //    //if (_foodService.CategoryProvider.GetAll().Count() != 0) return;
        //    //using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
        //    //{
        //    Categories = new List<Category>();
        //    SubCategories = new List<SubCategory>();
        //        foreach (var xCategory in _xCategoryData.Categories)
        //        {
        //            Category category = new Category();
        //            category.Name = xCategory.Name;
        //            category.ImageUri = _categoryImagesDirectory + xCategory.ImageUri;
        //            category.SubCategories = new List<SubCategory>();
        //            //_foodService.CategoryProvider.Create(category);
        //            foreach (var xSubCategory in xCategory.SubCategories)
        //            {
        //                SubCategory subCategory = new SubCategory();
        //                subCategory.Name = xSubCategory.Name;
        //                subCategory.Category = category;
        //                SubCategories.Add(subCategory);
        //                //_foodService.SubCategoryProvider.Create(subCategory);
        //                //category.SubCategories.Add(subCategory);
        //                //_foodService.SubCategoryProvider.Update(subCategory);
        //            }
        //            Categories.Add(category);
        //        }
        //    //}
        //}

        //private void ExtractFoodData()
        //{
        //    string imageUri, smallImageUri, middleImageUri, largeImageUri;

        //    //using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
        //    //{
        //    Foods = new List<Food>();
        //    NutritionalElements = new List<NutritionElement>();
        //    Introductions = new List<Introduction>();
        //        foreach (var xFood in _xFoodData.Foods)
        //        {
        //            //if (_foodService.FoodProvider.Get(xFood.Name) != null) continue;

        //            //using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
        //            //{
        //                //Category category = Categories.FirstOrDefault(c => c.Name == xFood.Categories[0].Value);
        //                //string categoryName = xFood.Categories[0].Value;
        //                //IEnumerable<Category> categories = _foodService.CategoryProvider.GetAll();
        //                //Category category = categories.FirstOrDefault(c => c.Name == categoryName);
        //                //Category category = _foodService.CategoryProvider.Get(xFood.Categories[0].Value);
        //            Category category = Categories.FirstOrDefault(c => c.Name == xFood.Categories[0].Value);
        //                if (category == null) continue;

        //                Food food = new Food();
        //                imageUri = xFood.ImageUri;
        //                string imageString = _foodImagesDirectory + imageUri;
        //                smallImageUri = imageString + "_small.jpg";
        //                middleImageUri = imageString + "_mid.jpg";
        //                largeImageUri = imageString + ".jpg";
        //                food.Name = xFood.Name;
        //                food.SmallImageUri = smallImageUri;
        //                food.MiddleImageUri = middleImageUri;
        //                food.LargeImageUri = largeImageUri;
        //                food.NutritionElements = new List<NutritionElement>();
        //                if (xFood.NutritionalElements.Length != 0)
        //                {
        //                    food.Calorie = xFood.NutritionalElements[0].Value;

        //                    //_foodService.FoodProvider.Create(food);
        //                    foreach (var xNutritionalElement in xFood.NutritionalElements)
        //                    {
        //                        NutritionElement nutritionalElement = new NutritionElement
        //                        {
        //                            Name = xNutritionalElement.Name,
        //                            Value = xNutritionalElement.Value,
        //                            Food = food
        //                        };
        //                        NutritionalElements.Add(nutritionalElement);
        //                        //_foodService.NutritionalElementProvider.Create(nutritionalElement);
        //                        //food.NutritionalElements.Add(nutritionalElement);
        //                        //_foodService.NutritionalElementProvider.Update(nutritionalElement);
        //                    }
        //                }
        //                else
        //                {
        //                    food.Calorie = new decimal(0.00);

        //                    //_foodService.FoodProvider.Create(food);

        //                    string[] name = new string[] { "热量(大卡)", "碳水化合物(克)", "脂肪(克)", "蛋白质(克)" };
        //                    for (int i = 0; i < 4; i++)
        //                    {
        //                        NutritionElement nutritionalElement = new NutritionElement
        //                        {
        //                            Name = name[i],
        //                            Value = new decimal(0.00),
        //                            Food = food
        //                        };
        //                        NutritionalElements.Add(nutritionalElement);
        //                        //_foodService.NutritionalElementProvider.Create(nutritionalElement);
        //                        //food.NutritionalElements.Add(nutritionalElement);
        //                        //_foodService.NutritionalElementProvider.Update(nutritionalElement);
        //                    }
        //                }

        //                if (xFood.Introductions.Length != 0)
        //                {
        //                    food.Introductions = new List<Introduction>();
        //                    foreach (var xIntroduction in xFood.Introductions)
        //                    {
        //                        if (xIntroduction.Content.Length > 999) continue;

        //                        Introduction introduction = new Introduction
        //                        {
        //                            Name = xIntroduction.Name,
        //                            Content = xIntroduction.Content,
        //                            Food = food
        //                        };
        //                        Introductions.Add(introduction);
        //                        //_foodService.IntroductionProvider.Create(introduction);
        //                        //food.Introductions.Add(introduction);
        //                        //_foodService.IntroductionProvider.Update(introduction);
        //                    }
        //                }

        //                //food.DietPlanItems = new List<DietPlanItem>();

        //                SubCategory subCategory = SubCategories.First(sc => sc.Name == xFood.Categories[1].Value);
        //                food.SubCategory = subCategory;
        //                //_foodService.FoodProvider.Update(food);

        //                Foods.Add(food);
        //            }
        //        //}
        //    //}
        //}

        private void ExtractDietPlanData()
        {
            //using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            //{
            DietPlans = new List<DietPlan>();
            SubDietPlans = new List<SubDietPlan>();
            DietPlanItems = new List<DietPlanItem>();
                foreach (var xDietPlan in _xDietPlanData.DietPlans)
                {
                    DietPlan dietPlan = new DietPlan();
                    dietPlan.Name = xDietPlan.Name;
                    dietPlan.CreatedDate = DateTime.Now;
                    dietPlan.PlanType = xDietPlan.PlanType ? PlanType.RecommendedDietPlan : PlanType.CreatedDietPlan;
                    dietPlan.SubDietPlans = new List<SubDietPlan>();
                    //_foodService.DietPlanProvider.Create(dietPlan);
                    foreach (var xSubDietPlan in xDietPlan.SubDietPlans)
                    {
                        SubDietPlan subDietPlan = new SubDietPlan();
                        subDietPlan.StartTime = DateTime.Now;
                        subDietPlan.DietPlanItems = new List<DietPlanItem>();
                        //_foodService.SubDietPlanProvider.Create(subDietPlan);
                        foreach (var xDietPlanItem in xSubDietPlan.DietPlanItems)
                        {
                            DietPlanItem dietPlanItem = new DietPlanItem();
                            dietPlanItem.Food = Foods.First(f => f.Name == xDietPlanItem.FoodName);
                            //dietPlanItem.Food = _foodService.FoodProvider.Get(xDietPlanItem.FoodName);
                            //_foodService.FoodProvider.Update(food);
                            //dietPlanItem.Food.DietPlanItems.Add(dietPlanItem);
                            dietPlanItem.Amount = xDietPlanItem.Amount;
                            dietPlanItem.SubDietPlan = subDietPlan;
                            DietPlanItems.Add(dietPlanItem);
                            //_foodService.DietPlanItemProvider.Create(dietPlanItem);
                            //subDietPlan.DietPlanItems.Add(dietPlanItem);
                            //_foodService.DietPlanItemProvider.Update(dietPlanItem);
                        }
                        //dietPlan.SubDietPlans.Add(subDietPlan);
                        //_foodService.SubDietPlanProvider.Update(subDietPlan);
                        subDietPlan.DietPlan = dietPlan;
                        SubDietPlans.Add(subDietPlan);
                    }
                    DietPlans.Add(dietPlan);
                }
            //}
        }

        public ICollection<Category> Categories { get; set; }

        public ICollection<Food> Foods { get; set; }

        public ICollection<NutritionElement> NutritionalElements { get; set; }

        public ICollection<DietPlan> DietPlans { get; set; }

        public ICollection<SubDietPlan> SubDietPlans { get; set; }

        public ICollection<DietPlanItem> DietPlanItems { get; set; }
    }
}
