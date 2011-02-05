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

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class DietPlanListViewModel : NotificationObject, IDietPlanListViewModel
    {
        private int _totalCalories;

        public DietPlanListViewModel(IDietPlanListView view)
        {
            View = view;
            View.Context = this;
            InitializeDietPlanList();
        }

        #region IDietPlanViewModel Members

        public IDietPlanListView View { get; set; }

        public int TotalCalories
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

        public IList<DietPlanSubListViewModel> DietPlanList { get; set; }

        #endregion

        private void InitializeDietPlanList()
        {
            _totalCalories = 0;

            DietPlanList = new List<DietPlanSubListViewModel>(6);

            IList<string> mealNames = new List<string> { "早餐", "上午加餐", "中餐", "中午加餐", "晚餐" , "晚上加餐"};

            for (int i = 0; i < 6; i++)
            {
                DietPlanSubListViewModel dietPlanSubList = new DietPlanSubListViewModel(mealNames[i]);
                dietPlanSubList.DietPlanListPropertyChanged += new EventHandler(DietPlanListPropertyChanged);
                DietPlanList.Add(dietPlanSubList);
            }
        }

        private void DietPlanListPropertyChanged(object sender, EventArgs e)
        {
            int totalCalories = 0;

            foreach (var dietPlanSubList in DietPlanList)
            {
                totalCalories += dietPlanSubList.SubTotalCalories;
            }

            TotalCalories = totalCalories;
        }
    }
}
