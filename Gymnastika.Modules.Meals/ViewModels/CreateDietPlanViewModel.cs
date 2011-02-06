using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class CreateDietPlanViewModel : ICreateDietPlanViewModel
    {
        private ICommand _saveCommand;

        public CreateDietPlanViewModel(ICreateDietPlanView view, IDietPlanListViewModel dietPlanListViewModel)
        {
            DietPlanListViewModel = dietPlanListViewModel;
            View = view;
            View.Context = this;
        }

        #region ICreateDietPlanViewModel Members

        public ICreateDietPlanView View { get; set; }

        public IDietPlanListViewModel DietPlanListViewModel { get; set; }

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

        }
    }
}
