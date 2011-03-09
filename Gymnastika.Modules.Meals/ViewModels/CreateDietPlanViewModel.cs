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
using Gymnastika.Data;
using Gymnastika.Services.Session;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class CreateDietPlanViewModel : NotificationObject, ICreateDietPlanViewModel
    {
        private readonly IFoodService _foodService;
        private readonly IWorkEnvironment _workEnvironment;
        private readonly ISessionManager _sessinManager;
        private DateTime _createdDate;
        private ICommand _saveCommand;

        public CreateDietPlanViewModel(ICreateDietPlanView view, 
            IDietPlanListViewModel dietPlanListViewModel,
            IFoodService foodService,
            ISessionManager sessionManager,
            IWorkEnvironment workEnvironment)
        {
            _foodService = foodService;
            _sessinManager = sessionManager;
            _workEnvironment = workEnvironment;
            CreatedDate = DateTime.Now;
            DietPlanListViewModel = dietPlanListViewModel;
            View = view;
            View.Context = this;
        }

        #region ICreateDietPlanViewModel Members

        public ICreateDietPlanView View { get; set; }

        private IDietPlanListViewModel _dietPlanListViewModel;
        public IDietPlanListViewModel DietPlanListViewModel
        {
            get
            {
                return _dietPlanListViewModel;
            }
            set
            {
                if (_dietPlanListViewModel != value)
                {
                    _dietPlanListViewModel = value;
                    RaisePropertyChanged("DietPlanListViewModel");
                }
            }
        }

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
            using (IWorkContextScope scope = _workEnvironment.GetWorkContextScope())
            {
                DietPlan = new DietPlan();
                DietPlan.User = _sessinManager.GetCurrentSession().AssociatedUser;
                DietPlan.PlanType = PlanType.CreatedDietPlan;
                DietPlan.CreatedDate = CreatedDate;
                DietPlan.SubDietPlans = new List<SubDietPlan>();
                _foodService.DietPlanProvider.Create(DietPlan);
                for (int i = 0; i < 6; i++)
                {
                    SubDietPlan subDietPlan = new SubDietPlan();
                    subDietPlan.DietPlan = DietPlan;
                    subDietPlan.DietPlanItems = new List<DietPlanItem>();
                    _foodService.SubDietPlanProvider.Create(subDietPlan);
                    foreach (var foodItem in DietPlanListViewModel.DietPlanList[i].DietPlanSubList)
                    {
                        DietPlanItem dietPlanItem = new DietPlanItem();
                        dietPlanItem.Food = foodItem.Food;
                        dietPlanItem.Amount = foodItem.Amount;
                        dietPlanItem.SubDietPlan = subDietPlan;
                        _foodService.DietPlanItemProvider.Create(dietPlanItem);
                        //subDietPlan.DietPlanItems.Add(dietPlanItem);
                        //_foodService.DietPlanItemProvider.Update(dietPlanItem);
                    }
                    //DietPlan.SubDietPlans.Add(subDietPlan);
                    //_foodService.SubDietPlanProvider.Update(subDietPlan);
                }
            }
            System.Windows.MessageBox.Show("已保存");
        }
    }
}
