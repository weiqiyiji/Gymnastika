using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Gymnastika.Modules.Sports.Models;
namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface ISportsListViewModel
    {
        ObservableCollection<Category> Categories { get; set; }
        Category SelectedCategory { get; }
    }
}
