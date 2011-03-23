using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Services;
using Gymnastika.Data;
using Gymnastika.Modules.Meals.Views;
using Microsoft.Practices.Prism.Regions;
using Gymnastika.Common;
using System.Threading;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Controllers
{
    public class LoadDataController : ILoadDataController
    {
        private readonly IFoodService _foodService;
        private readonly IWorkEnvironment _workEnviroment;
        private readonly LoadDataView _loadDataView;
        private readonly XDataHelpers.XDataRepository _dataSource;

        public LoadDataController(IFoodService foodService, IWorkEnvironment workEnvironment, LoadDataView loadDataView)
        {
            _foodService = foodService;
            _workEnviroment = workEnvironment;
            _loadDataView = loadDataView;

            _dataSource = new XDataHelpers.XDataRepository(_foodService, _workEnviroment);
            _dataSource.ExtractDatas();
        }

        #region ILoadDataController Members

        private bool _isLoaded;
        public bool IsLoaded
        {
            get 
            {
                using (IWorkContextScope scope = _workEnviroment.GetWorkContextScope())
                {
                    _isLoaded = (_foodService.CategoryProvider.count() != 0);
                }
                return _isLoaded;
            }
        }

        public void Load()
        {
            LoadCategoryData();
            LoadFoodData();
            LoadNutritionalElementData();
        }

        #endregion

        public void LoadCategoryData()
        {
            using (IWorkContextScope scope = _workEnviroment.GetWorkContextScope())
            {
                foreach (var category in _dataSource.Categories)
                {
                    _foodService.CategoryProvider.Create(category);
                }
            }
        }

        public void LoadFoodData()
        {
            using (IWorkContextScope scope = _workEnviroment.GetWorkContextScope())
            {
                foreach (var food in _dataSource.Foods)
                {
                    _foodService.FoodProvider.Create(food);
                }
            }
        }

        public void LoadNutritionalElementData()
        {
            using (IWorkContextScope scope = _workEnviroment.GetWorkContextScope())
            {
                foreach (var nutrionalElement in _dataSource.NutritionalElements)
                {
                    _foodService.NutritionElementProvider.Create(nutrionalElement);
                }
            }
        }

        public void LoadDietPlanData()
        {
            using (IWorkContextScope scope = _workEnviroment.GetWorkContextScope())
            {
                foreach (var dietPlan in _dataSource.DietPlans)
                {
                    _foodService.DietPlanProvider.Create(dietPlan);
                }
            }
        }

        public void LoadSubDietPlanData()
        {
            using (IWorkContextScope scope = _workEnviroment.GetWorkContextScope())
            {
                foreach (var subDietPlan in _dataSource.SubDietPlans)
                {
                    _foodService.SubDietPlanProvider.Create(subDietPlan);
                }
            }
        }

        public void LoadDietPlanItemData()
        {
            using (IWorkContextScope scope = _workEnviroment.GetWorkContextScope())
            {
                foreach (var dietPlanItem in _dataSource.DietPlanItems)
                {
                    _foodService.DietPlanItemProvider.Create(dietPlanItem);
                }
            }
        }
    }
}
