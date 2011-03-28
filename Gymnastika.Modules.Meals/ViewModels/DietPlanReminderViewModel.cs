using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class DietPlanReminderViewModel
    {
        public DietPlanReminderViewModel(DietPlanReminderView view)
        {
            View = view;
            view.Context = this;
        }

        #region IDietPlanReminderViewModel Members

        public DietPlanReminderView View { get; set; }

        public IList<string> NutritionNames { get; set; }

        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new DelegateCommand(Close);

                return _closeCommand;
            }
        }

        #endregion

        private void Close()
        {
            View.Close();
        }
    }
}
