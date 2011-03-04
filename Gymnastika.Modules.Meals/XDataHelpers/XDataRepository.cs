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

namespace Gymnastika.Modules.Meals.XDataHelpers
{
    public class XDataRepository
    {
        private readonly XDataFileManager _dataFileManager;
        private readonly XCategoryData _xCategoryData;
        private readonly XFoodData _xFoodData;
        private readonly XDietPlanData _xDietPlanData;
        private readonly IFoodService _foodService;
        private readonly string _categoryImagesDirectory;
        private readonly string _foodImagesDirectory;

        public XDataRepository(IFoodService foodService)
        {
            _foodService = foodService;

            if (_foodService.CategoryProvider.GetAll().Count() != 0) return;

            _dataFileManager = new XDataFileManager();

            _xCategoryData = _dataFileManager.GetCategoryData();
            _xFoodData = _dataFileManager.GetFoodData();
            _xDietPlanData = _dataFileManager.GetDietPlanData();

            string currentDirectory = Directory.GetCurrentDirectory();
            string imagesDirectory = currentDirectory + "\\Images\\";
            _categoryImagesDirectory = imagesDirectory + "CategoryImages\\";
            _foodImagesDirectory = imagesDirectory + "FoodImages\\";

            ////Categories = new List<Category>();
            //foreach (var xCategory in _xCategoryData.Categories)
            //{
            //    Category category = new Category();
            //    category.Name = xCategory.Name;
            //    category.ImageUri = categoryImagesDirectory + xCategory.ImageUri;
            //    category.SubCategories = new List<SubCategory>();
            //    _foodService.CategoryProvider.Create(category);
            //    foreach (var xSubCategory in xCategory.SubCategories)
            //    {
            //        SubCategory subCategory = new SubCategory();
            //        subCategory.Name = xSubCategory.Name;
            //        subCategory.Category = category;
            //        _foodService.SubCategoryProvider.Create(subCategory);
            //        category.SubCategories.Add(subCategory);
            //        _foodService.SubCategoryProvider.Update(subCategory);
            //    }
            //    //Categories.Add(category);
            //}


            ////Foods = new List<Food>();
            //string imageUri, smallImageUri, middleImageUri, largeImageUri;
            //foreach (var xFood in _xFoodData.Foods)
            //{
            //    //Category category = Categories.FirstOrDefault(c => c.Name == xFood.Categories[0].Value);
            //    Category category = _foodService.CategoryProvider.Get(xFood.Categories[0].Value);
            //    if (category == null) continue;

            //    Food food = new Food();
            //    imageUri = xFood.ImageUri;
            //    string imageString = foodImagesDirectory + imageUri;
            //    smallImageUri = imageString + "_small.jpg";
            //    middleImageUri = imageString + "_mid.jpg";
            //    largeImageUri = imageString + ".jpg";
            //    food.Name = xFood.Name;
            //    food.SmallImageUri = smallImageUri;
            //    food.MiddleImageUri = middleImageUri;
            //    food.LargeImageUri = largeImageUri;
            //    food.NutritionalElements = new List<NutritionalElement>();
            //    _foodService.FoodProvider.Create(food);
            //    if (xFood.NutritionalElements.Length != 0)
            //    {
            //        food.Calorie = xFood.NutritionalElements[0].Value;

            //        foreach (var xNutritionalElement in xFood.NutritionalElements)
            //        {
            //            NutritionalElement nutritionalElement = new NutritionalElement
            //            {
            //                Name = xNutritionalElement.Name,
            //                Value = xNutritionalElement.Value,
            //                Food = food
            //            };
            //            _foodService.NutritionalElementProvider.Create(nutritionalElement);
            //            food.NutritionalElements.Add(nutritionalElement);
            //            _foodService.NutritionalElementProvider.Update(nutritionalElement);
            //        }
            //    }
            //    else
            //    {
            //        food.Calorie = new decimal(0.00);

            //        string[] name = new string[] { "热量(大卡)", "碳水化合物(克)", "脂肪(克)", "蛋白质(克)" };
            //        for (int i = 0; i < 4; i++)
            //        {
            //            NutritionalElement nutritionalElement = new NutritionalElement
            //            {
            //                Name = name[i],
            //                Value = new decimal(0.00),
            //                Food = food
            //            };
            //            _foodService.NutritionalElementProvider.Create(nutritionalElement);
            //            food.NutritionalElements.Add(nutritionalElement);
            //            _foodService.NutritionalElementProvider.Update(nutritionalElement);
            //        }
            //    }
                
            //    if (xFood.Introductions != null)
            //    {
            //        food.Introductions = new List<Introduction>();
            //        foreach (var xIntroduction in xFood.Introductions)
            //        {
            //            Introduction introduction = new Introduction
            //            {
            //                Name = xIntroduction.Name,
            //                Content = xIntroduction.Content,
            //                Food = food
            //            };
            //            _foodService.IntroductionProvider.Create(introduction);
            //            food.Introductions.Add(introduction);
            //            _foodService.IntroductionProvider.Update(introduction);
            //        }
            //    }

            //    //food.DietPlanItems = new List<DietPlanItem>();

            //    SubCategory subCategory = category.SubCategories.First(sc => sc.Name == xFood.Categories[1].Value);
            //    food.SubCategory = subCategory;
            //    _foodService.FoodProvider.Update(food);

            //    //Foods.Add(food);
            //}

            //foreach (var category in Categories)
            //{
            //    foreach (var subCategory in category.SubCategories)
            //    {
            //        IList<Food> foods = new List<Food>();
            //        foods = Foods.Where(f => f.SubCategory.Name == subCategory.Name).ToList();
            //        subCategory.Foods = foods;
            //    }
            //}

            //DietPlans = new List<DietPlan>();
            //foreach (var xDietPlan in _xDietPlanData.DietPlans)
            //{
            //    DietPlan dietPlan = new DietPlan();
            //    dietPlan.Name = xDietPlan.Name;
            //    dietPlan.PlanType = xDietPlan.PlanType ? PlanType.RecommendedDietPlan : PlanType.CreatedDietPlan;
            //    dietPlan.SubDietPlans = new List<SubDietPlan>();
            //    _foodService.DietPlanProvider.Create(dietPlan);
            //    foreach (var xSubDietPlan in xDietPlan.SubDietPlans)
            //    {
            //        SubDietPlan subDietPlan = new SubDietPlan();
            //        subDietPlan.DietPlanItems = new List<DietPlanItem>();
            //        _foodService.SubDietPlanProvider.Create(subDietPlan);
            //        foreach (var xDietPlanItem in xSubDietPlan.DietPlanItems)
            //        {
            //            DietPlanItem dietPlanItem = new DietPlanItem();
            //            //dietPlanItem.Food = Foods.First(f => f.Name == xDietPlanItem.FoodName);
            //            Food food = _foodService.FoodProvider.Get(xDietPlanItem.FoodName);
            //            dietPlanItem.Food = food;
            //            _foodService.FoodProvider.Update(food);
            //            //dietPlanItem.Food.DietPlanItems.Add(dietPlanItem);
            //            dietPlanItem.Amount = xDietPlanItem.Amount;
            //            _foodService.DietPlanItemProvider.Create(dietPlanItem);
            //            subDietPlan.DietPlanItems.Add(dietPlanItem);
            //            _foodService.DietPlanItemProvider.Update(dietPlanItem);
            //            //dietPlanItem.SubDietPlan = subDietPlan;
            //        }
            //        dietPlan.SubDietPlans.Add(subDietPlan);
            //        _foodService.SubDietPlanProvider.Update(subDietPlan);
            //        //subDietPlan.DietPlan = dietPlan;
            //    }
            //    //DietPlans.Add(dietPlan);
            //}
        }

        public void BeginStore()
        {
            StoreCategories();
            StoreFoods();
            StoreDietPlans();
        }

        private void StoreCategories()
        {
            foreach (var xCategory in _xCategoryData.Categories)
            {
                Category category = new Category();
                category.Name = xCategory.Name;
                category.ImageUri = _categoryImagesDirectory + xCategory.ImageUri;
                category.SubCategories = new List<SubCategory>();
                _foodService.CategoryProvider.Create(category);
                foreach (var xSubCategory in xCategory.SubCategories)
                {
                    SubCategory subCategory = new SubCategory();
                    subCategory.Name = xSubCategory.Name;
                    subCategory.Category = category;
                    _foodService.SubCategoryProvider.Create(subCategory);
                    category.SubCategories.Add(subCategory);
                    _foodService.SubCategoryProvider.Update(subCategory);
                }
                //Categories.Add(category);
            }
        }

        private void StoreFoods()
        {
            string imageUri, smallImageUri, middleImageUri, largeImageUri;
            foreach (var xFood in _xFoodData.Foods)
            {
                //Category category = Categories.FirstOrDefault(c => c.Name == xFood.Categories[0].Value);
                Category category = _foodService.CategoryProvider.Get(xFood.Categories[0].Value);
                if (category == null) continue;

                Food food = new Food();
                imageUri = xFood.ImageUri;
                string imageString = _foodImagesDirectory + imageUri;
                smallImageUri = imageString + "_small.jpg";
                middleImageUri = imageString + "_mid.jpg";
                largeImageUri = imageString + ".jpg";
                food.Name = xFood.Name;
                food.SmallImageUri = smallImageUri;
                food.MiddleImageUri = middleImageUri;
                food.LargeImageUri = largeImageUri;
                food.NutritionalElements = new List<NutritionalElement>();
                _foodService.FoodProvider.Create(food);
                if (xFood.NutritionalElements.Length != 0)
                {
                    food.Calorie = xFood.NutritionalElements[0].Value;

                    foreach (var xNutritionalElement in xFood.NutritionalElements)
                    {
                        NutritionalElement nutritionalElement = new NutritionalElement
                        {
                            Name = xNutritionalElement.Name,
                            Value = xNutritionalElement.Value,
                            Food = food
                        };
                        _foodService.NutritionalElementProvider.Create(nutritionalElement);
                        food.NutritionalElements.Add(nutritionalElement);
                        _foodService.NutritionalElementProvider.Update(nutritionalElement);
                    }
                }
                else
                {
                    food.Calorie = new decimal(0.00);

                    string[] name = new string[] { "热量(大卡)", "碳水化合物(克)", "脂肪(克)", "蛋白质(克)" };
                    for (int i = 0; i < 4; i++)
                    {
                        NutritionalElement nutritionalElement = new NutritionalElement
                        {
                            Name = name[i],
                            Value = new decimal(0.00),
                            Food = food
                        };
                        _foodService.NutritionalElementProvider.Create(nutritionalElement);
                        food.NutritionalElements.Add(nutritionalElement);
                        _foodService.NutritionalElementProvider.Update(nutritionalElement);
                    }
                }

                if (xFood.Introductions != null)
                {
                    food.Introductions = new List<Introduction>();
                    foreach (var xIntroduction in xFood.Introductions)
                    {
                        Introduction introduction = new Introduction
                        {
                            Name = xIntroduction.Name,
                            Content = xIntroduction.Content,
                            Food = food
                        };
                        _foodService.IntroductionProvider.Create(introduction);
                        food.Introductions.Add(introduction);
                        _foodService.IntroductionProvider.Update(introduction);
                    }
                }

                //food.DietPlanItems = new List<DietPlanItem>();

                SubCategory subCategory = category.SubCategories.First(sc => sc.Name == xFood.Categories[1].Value);
                food.SubCategory = subCategory;
                _foodService.FoodProvider.Update(food);

                //Foods.Add(food);
            }
        }

        private void StoreDietPlans()
        {
            foreach (var xDietPlan in _xDietPlanData.DietPlans)
            {
                DietPlan dietPlan = new DietPlan();
                dietPlan.Name = xDietPlan.Name;
                dietPlan.PlanType = xDietPlan.PlanType ? PlanType.RecommendedDietPlan : PlanType.CreatedDietPlan;
                dietPlan.SubDietPlans = new List<SubDietPlan>();
                _foodService.DietPlanProvider.Create(dietPlan);
                foreach (var xSubDietPlan in xDietPlan.SubDietPlans)
                {
                    SubDietPlan subDietPlan = new SubDietPlan();
                    subDietPlan.DietPlanItems = new List<DietPlanItem>();
                    _foodService.SubDietPlanProvider.Create(subDietPlan);
                    foreach (var xDietPlanItem in xSubDietPlan.DietPlanItems)
                    {
                        DietPlanItem dietPlanItem = new DietPlanItem();
                        //dietPlanItem.Food = Foods.First(f => f.Name == xDietPlanItem.FoodName);
                        Food food = _foodService.FoodProvider.Get(xDietPlanItem.FoodName);
                        dietPlanItem.Food = food;
                        _foodService.FoodProvider.Update(food);
                        //dietPlanItem.Food.DietPlanItems.Add(dietPlanItem);
                        dietPlanItem.Amount = xDietPlanItem.Amount;
                        _foodService.DietPlanItemProvider.Create(dietPlanItem);
                        subDietPlan.DietPlanItems.Add(dietPlanItem);
                        _foodService.DietPlanItemProvider.Update(dietPlanItem);
                        //dietPlanItem.SubDietPlan = subDietPlan;
                    }
                    dietPlan.SubDietPlans.Add(subDietPlan);
                    _foodService.SubDietPlanProvider.Update(subDietPlan);
                    //subDietPlan.DietPlan = dietPlan;
                }
                //DietPlans.Add(dietPlan);
            }
        }

        //public ICollection<Category> Categories { get; set; }

        //public ICollection<Food> Foods { get; set; }

        //public ICollection<DietPlan> DietPlans { get; set; }
    }
}
