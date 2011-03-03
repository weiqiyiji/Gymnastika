using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Gymnastika.Common.Extensions;

namespace Gymnastika.Modules.Sports.Extensions
{
    public static class ListExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> list)
        {
            return new ObservableCollection<T>(list);
        }
        public static IList<T> ReplaceBy<T>(this IList<T> list, IList<T> source)
        {
            list.Clear();
            list.AddRange(source);
            return list;
        }
    }

}
