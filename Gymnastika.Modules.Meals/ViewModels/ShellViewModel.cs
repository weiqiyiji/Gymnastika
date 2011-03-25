using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class ShellViewModel : NotificationObject
    {
        private IMealsManagementViewModel _mealsManagementViewModel;
        public IMealsManagementViewModel MealsManangementViewModel 
        {
            get
            {
                return _mealsManagementViewModel;
            }
            set
            {
                if (_mealsManagementViewModel != null)
                {
                    _mealsManagementViewModel = value;
                    RaisePropertyChanged("MealsManangementViewModel");
                }
            }
        }
    }
}
