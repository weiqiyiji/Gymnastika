using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.ViewModels;
using System.Windows.Data;
using System.Windows.Input;

namespace Gymnastika.Modules.Meals.Views
{
    public interface IMealsManagementView
    {
        IMealsManagementViewModel Context { get; set; }
        BindingExpression GetBindingSearchString();
        event KeyEventHandler SearchKeyDown;
    }
}
