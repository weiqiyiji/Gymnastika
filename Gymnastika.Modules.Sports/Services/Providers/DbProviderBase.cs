using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;
using Microsoft.Practices.Unity;

namespace Gymnastika.Modules.Sports.Services.Providers
{
    public interface IProvider<T>
    {
        IEnumerable<T> Fetch(Func<T, bool> predicate);

        IEnumerable<T> Fetch(int startIndex, int number);

        void CreateOrUpdate(T entity);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        IEnumerable<T> All();

        IWorkContextScope GetContextScope();
    }

    public class DbProviderBase<T> : IProvider<T>
    {
        [Dependency]
        public IRepository<T> Repository { get; set; }
        
        [Dependency]
        public IWorkEnvironment Environment { get; set; }
        
        public DbProviderBase()
        {

        }

        public virtual IEnumerable<T> Fetch(Func<T, bool> predicate)
        {
            return Repository.Fetch(t => predicate(t));
        }

        public virtual IEnumerable<T> Fetch(int startIndex, int number)
        {
            return Repository.Fetch(t => true).Take(number).Skip(startIndex);
        }

        public virtual void CreateOrUpdate(T entity)
        {
            Repository.CreateOrUpdate(entity);
        }
        public virtual void Create(T entity)
        {
            Repository.Create(entity);
        }

        public virtual void Update(T entity)
        {
            Repository.Update(entity);
        }
        public virtual void Delete(T entity)
        {
            Repository.Delete(entity);
        }

        public virtual IEnumerable<T> All()
        {
            return Fetch((t) => true);
        }

        #region IProvider<T> Members


        public IWorkContextScope GetContextScope()
        {
            return Environment.GetWorkContextScope();
        }

        #endregion
    }
}
