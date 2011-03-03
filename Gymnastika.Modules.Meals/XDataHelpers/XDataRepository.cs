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

namespace Gymnastika.Modules.Meals.XDataHelpers
{
    public class XDataRepository
    {
        private readonly XDataFileManager _dataFileManager;
        private readonly XCategoryData _xCategoryData;
        private readonly XFoodData _xFoodData;
        private readonly XDietPlanData _xDietPlanData;

        public XDataRepository()
        {
            _dataFileManager = new XDataFileManager();

            _xCategoryData = _dataFileManager.GetCategoryData();
            _xFoodData = _dataFileManager.GetFoodData();
            _xDietPlanData = _dataFileManager.GetDietPlanData();


            string currentDirectory = Directory.GetCurrentDirectory();
            string imagesDirectory = currentDirectory + "\\Images\\";
            string categoryImagesDirectory = imagesDirectory + "CategoryImages\\";
            string foodImagesDirectory = imagesDirectory + "FoodImages\\";

            Categories = new List<Category>();
            foreach (var xCategory in _xCategoryData.Categories)
            {
                Category category = new Category();
                category.Name = xCategory.Name;
                category.ImageUri = categoryImagesDirectory + xCategory.ImageUri;
                category.SubCategories = new List<SubCategory>();
                foreach (var xSubCategory in xCategory.SubCategories)
                {
                    SubCategory subCategory = new SubCategory();
                    subCategory.Name = xSubCategory.Name;
                    subCategory.Category = category;
                    category.SubCategories.Add(subCategory);
                }
                Categories.Add(category);
            }


            Foods = new List<Food>();
            string imageUri, smallImageUri, middleImageUri, largeImageUri;
            foreach (var xFood in _xFoodData.Foods)
            {
                Food food = new Food();
                imageUri = xFood.ImageUri;
                string imageString = foodImagesDirectory + imageUri;
                smallImageUri = imageString + "_small.jpg";
                middleImageUri = imageString + "_mid.jpg";
                largeImageUri = imageString + ".jpg";
                food.Name = xFood.Name;
                food.SmallImageUri = smallImageUri;
                food.MiddleImageUri = middleImageUri;
                food.LargeImageUri = largeImageUri;
                food.NutritionalElements = new List<NutritionalElement>();
                if (xFood.NutritionalElements.Length != 0)
                {
                    food.Calorie = xFood.NutritionalElements[0].Value;

                    foreach (var xNutritionalElement in xFood.NutritionalElements)
                    {
                        food.NutritionalElements.Add(new NutritionalElement
                        {
                            Name = xNutritionalElement.Name,
                            Value = xNutritionalElement.Value,
                            Food = food
                        });
                    }
                }
                else
                {
                    food.Calorie = new decimal(0.00);

                    string[] name = new string[] { "热量(大卡)", "碳水化合物(克)", "脂肪(克)", "蛋白质(克)" };
                    for (int i = 0; i < 4; i++)
                    {
                        food.NutritionalElements.Add(new NutritionalElement
                               {
                                   Name = name[i],
                                   Value = new decimal(0.00),
                                   Food = food
                               });
                    }
                }
                
                if (xFood.Introductions != null)
                {
                    food.Introductions = new List<Introduction>();
                    foreach (var xIntroduction in xFood.Introductions)
                    {
                        food.Introductions.Add(new Introduction
                        {
                            Name = xIntroduction.Name,
                            Content = xIntroduction.Content,
                            Food = food
                        });
                    }
                }

                food.DietPlanItems = new List<DietPlanItem>();

                Category category = Categories.FirstOrDefault(c => c.Name == xFood.Categories[0].Value);
                if (category == null) continue;
                SubCategory subCategory = category.SubCategories.First(sc => sc.Name == xFood.Categories[1].Value);
                food.SubCategory = subCategory;

                Foods.Add(food);
            }

            foreach (var category in Categories)
            {
                foreach (var subCategory in category.SubCategories)
                {
                    IList<Food> foods = new List<Food>();
                    foods = Foods.Where(f => f.SubCategory.Name == subCategory.Name).ToList();
                    subCategory.Foods = foods;
                }
            }

            DietPlans = new List<DietPlan>();
            foreach (var xDietPlan in _xDietPlanData.DietPlans)
            {
                DietPlan dietPlan = new DietPlan();
                dietPlan.Name = xDietPlan.Name;
                dietPlan.PlanType = xDietPlan.PlanType ? PlanType.RecommendedDietPlan : PlanType.CreatedDietPlan;
                dietPlan.SubDietPlans = new List<SubDietPlan>();
                foreach (var xSubDietPlan in xDietPlan.SubDietPlans)
                {
                    SubDietPlan subDietPlan = new SubDietPlan();
                    subDietPlan.DietPlanItems = new List<DietPlanItem>();
                    foreach (var xDietPlanItem in xSubDietPlan.DietPlanItems)
                    {
                        DietPlanItem dietPlanItem = new DietPlanItem();
                        dietPlanItem.Food = Foods.First(f => f.Name == xDietPlanItem.FoodName);
                        dietPlanItem.Food.DietPlanItems.Add(dietPlanItem);
                        dietPlanItem.Amount = xDietPlanItem.Amount;
                        subDietPlan.DietPlanItems.Add(dietPlanItem);
                    }
                    subDietPlan.DietPlan = dietPlan;
                    dietPlan.SubDietPlans.Add(subDietPlan);
                }
                DietPlans.Add(dietPlan);
            }
        }

        public ICollection<Category> Categories { get; set; }

        public ICollection<Food> Foods { get; set; }

        public ICollection<DietPlan> DietPlans { get; set; }
    }
}
