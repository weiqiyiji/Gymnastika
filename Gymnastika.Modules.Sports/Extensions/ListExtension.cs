using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Gymnastika.Modules.Sports.Extensions
{
    public static class ListExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> list)
        {
            return new ObservableCollection<T>(list);
        }
    }

}
