using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Gymnastika.Common.Extensions
{
    public static class ListExtensions
    {
        public static void ForEach(this IEnumerable list, Action<object> action)
        {
            if (action == null) throw new ArgumentNullException("action");

            foreach (var obj in list)
            {
                action(obj);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            if (action == null) throw new ArgumentNullException("action");

            foreach (var item in list)
            {
                action(item);
            }
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> enumrable)
        {
            if (enumrable == null) throw new ArgumentNullException("enumerable");

            enumrable.ForEach(t => list.Add(t));
        }
    }
}
