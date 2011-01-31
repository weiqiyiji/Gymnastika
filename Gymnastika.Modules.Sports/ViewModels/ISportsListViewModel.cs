using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Controls;
namespace Gymnastika.Modules.Sports.ViewModels
{
    public delegate ObservableCollection<object> GetSearchResults(string query);
    public interface ISportsListViewModel
    {
        ObservableCollection<Category> Categories { get; set; }
    }
}
