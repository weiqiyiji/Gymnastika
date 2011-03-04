using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Sports.Services
{
    public interface IProvider<T>
    {
        IEnumerable<T> Fetch(Func<T, bool> predicate);

        IEnumerable<T> Fetch(int startIndex, int number);

        void CreateOrUpdate(T entity);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

    }
}
