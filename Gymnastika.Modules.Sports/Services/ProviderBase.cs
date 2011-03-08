using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;

namespace Gymnastika.Modules.Sports.Services
{
    public class ProviderBase<T> : IProvider<T>
    {
        IRepository<T> _repository;
        IWorkEnvironment _environment;
        
        public ProviderBase(IRepository<T> repository,IWorkEnvironment environment)
        {
            _repository = repository;
            _environment = environment;
        }

        public virtual IEnumerable<T> Fetch(Func<T, bool> predicate)
        {
            return _repository.Fetch(t => predicate(t));
        }

        public virtual IEnumerable<T> Fetch(int startIndex, int number)
        {
            return _repository.Fetch(t => true).Take(number).Skip(startIndex);
        }

        public virtual void CreateOrUpdate(T entity)
        {
            _repository.CreateOrUpdate(entity);
        }
        public virtual void Create(T entity)
        {
            _repository.Create(entity);
        }

        public virtual void Update(T entity)
        {
            _repository.Update(entity);
        }
        public virtual void Delete(T entity)
        {
            _repository.Delete(entity);
        }

        public virtual IEnumerable<T> All()
        {
            return Fetch((t) => true);
        }

        #region IProvider<T> Members


        public IWorkContextScope GetContextScope()
        {
            return _environment.GetWorkContextScope();
        }

        #endregion
    }
}
