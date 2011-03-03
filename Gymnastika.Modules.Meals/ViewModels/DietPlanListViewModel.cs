using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Gymnastika.Modules.Meals.Models;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class DietPlanListViewModel : NotificationObject, IDietPlanListViewModel
    {
        private decimal _totalCalories;

        public DietPlanListViewModel(IDietPlanListView view)
        {
            View = view;
            View.Context = this;
            InitializeDietPlanList();
        }

        #region IDietPlanViewModel Members

        public IDietPlanListView View { get; set; }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }

        public decimal TotalCalories
        {
            get
            {
                return _totalCalories;
            }
            set
            {
                if (_totalCalories != value)
                {
                    _totalCalories = value;
                    RaisePropertyChanged("TotalCalories");
                }
            }
        }

        public IList<IDietPlanSubListViewModel> DietPlanList { get; set; }

        #endregion

        private void InitializeDietPlanList()
        {
            _totalCalories = 0;

            DietPlanList = new List<IDietPlanSubListViewModel>(6);

            IList<string> mealNames = new List<string> { "早餐", "上午加餐", "中餐", "中午加餐", "晚餐" , "晚上加餐"};

            for (int i = 0; i < 6; i++)
            {
                IDietPlanSubListViewModel dietPlanSubList = ServiceLocator.Current.GetInstance<IDietPlanSubListViewModel>();
                dietPlanSubList.MealName = mealNames[i];
                dietPlanSubList.DietPlanListPropertyChanged += new EventHandler(DietPlanListPropertyChanged);
                DietPlanList.Add(dietPlanSubList);
                dietPlanSubList.View.Expand();
            }
        }

        private void DietPlanListPropertyChanged(object sender, EventArgs e)
        {
            decimal totalCalories = 0;

            foreach (var dietPlanSubList in DietPlanList)
            {
                totalCalories += dietPlanSubList.SubTotalCalories;
            }

            TotalCalories = totalCalories;
        }
    }
}
