using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Gymnastika.Modules.Meals.Models;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Meals.Services;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class CreateDietPlanViewModel : NotificationObject, ICreateDietPlanViewModel
    {
        private readonly IFoodService _foodService;
        private DateTime _createdDate;
        private ICommand _saveCommand;

        public CreateDietPlanViewModel(ICreateDietPlanView view, 
            IDietPlanListViewModel dietPlanListViewModel
            //,
            //IFoodService foodService
            )
        {
            DietPlanListViewModel = dietPlanListViewModel;
            //_foodService = foodService;
            CreatedDate = DateTime.Now;
            View = view;
            View.Context = this;
        }

        #region ICreateDietPlanViewModel Members

        public ICreateDietPlanView View { get; set; }

        public IDietPlanListViewModel DietPlanListViewModel { get; set; }

        public DietPlan DietPlan { get; set; }

        public DateTime CreatedDate
        {
            get
            {
                return _createdDate;
            }
            set
            {
                if (_createdDate != value)
                {
                    _createdDate = value;
                    RaisePropertyChanged("CreatedDate");
                }
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new DelegateCommand(Save);

                return _saveCommand;
            }
        }

        #endregion

        private void Save()
        {
            IList<SubDietPlan> subDietPlans = new List<SubDietPlan>();

            for (int i = 0; i < 6; i++)
            {
                foreach (var foodItem in DietPlanListViewModel.DietPlanList[i].DietPlanSubList)
                {
                    subDietPlans[i].DietPlanItems.Add(new DietPlanItem
                    {
                        Food = foodItem.Food,
                        Amount = foodItem.Amount
                    });
                }
            }

            DietPlan.SubDietPlans = subDietPlans;
            DietPlan.PlanType = PlanType.CreatedDietPlan;
            DietPlan.CreatedDate = CreatedDate;

            _foodService.CreateDietPlan(DietPlan);
        }
    }
}
