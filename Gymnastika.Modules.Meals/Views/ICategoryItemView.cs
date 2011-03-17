using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.ViewModels;
using System.Windows.Controls;

namespace Gymnastika.Modules.Meals.Views
{
    public interface ICategoryItemView
    {
        ICategoryItemViewModel Context { get; set; }
        event SelectionChangedEventHandler SubCategoryItemSelectionChanged;
    }
}
