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
        private readonly IRegionManager _regionManager;
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

        public bool IsLoaded
        {
            get 
            { 
                return false;
            }
        }

        public void Load()
        {
            //ActiveLoadDataView();

            //ThreadDelegate backGroundLoader = new ThreadDelegate(_dataSource.Store);
            //backGroundLoader.BeginInvoke(null, null);


            //Thread.Sleep(60 * 60 * 1000);

            //_loadDataView.Close();

            //LoadCategoryData();
            //LoadFoodData();
            //LoadDietPlanData();
        }

        #endregion

        private delegate void ThreadDelegate();

        private void ActiveLoadDataView()
        {
            //IRegion displayRegion = _regionManager.Regions[RegionNames.DisplayRegion];

            //displayRegion.Add(_loadDataView);
            //displayRegion.Activate(_loadDataView);
            _loadDataView.Show();
        }

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

        public void LoadSubCategoryData()
        {
            using (IWorkContextScope scope = _workEnviroment.GetWorkContextScope())
            {
                foreach (var subCategory in _dataSource.SubCategories)
                {
                    _foodService.SubCategoryProvider.Create(subCategory);
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
            //using (IWorkContextScope scope = _workEnviroment.GetWorkContextScope())
            //{
            //    foreach (var nutrionalElement in _dataSource.NutritionalElements)
            //    {
            //        _foodService.NutritionalElementProvider.Create(nutrionalElement);
            //    }
            //}
            IList<NutritionalElement> nutritionalElements = new List<NutritionalElement>(_dataSource.NutritionalElements);
            
            using (IWorkContextScope scope = _workEnviroment.GetWorkContextScope())
            {
                for (int i = 0; i < 10000; i++)
                {
                    _foodService.NutritionalElementProvider.Create(nutritionalElements[i]);
                }
            }
            using (IWorkContextScope scope = _workEnviroment.GetWorkContextScope())
            {
                for (int i = 10000; i < 20000; i++)
                {
                    _foodService.NutritionalElementProvider.Create(nutritionalElements[i]);
                }
            }
            using (IWorkContextScope scope = _workEnviroment.GetWorkContextScope())
            {
                for (int i = 20000; i < 30000; i++)
                {
                    _foodService.NutritionalElementProvider.Create(nutritionalElements[i]);
                }
            }
            using (IWorkContextScope scope = _workEnviroment.GetWorkContextScope())
            {
                for (int i = 30000; i < 40000; i++)
                {
                    _foodService.NutritionalElementProvider.Create(nutritionalElements[i]);
                }
            }
            using (IWorkContextScope scope = _workEnviroment.GetWorkContextScope())
            {
                for (int i = 40000; i < 45532; i++)
                {
                    _foodService.NutritionalElementProvider.Create(nutritionalElements[i]);
                }
            }
        }
        
        public void LoadIntroductionData()
        {
            using (IWorkContextScope scope = _workEnviroment.GetWorkContextScope())
            {
                foreach (var introduction in _dataSource.Introductions)
                {
                    _foodService.IntroductionProvider.Create(introduction);
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
