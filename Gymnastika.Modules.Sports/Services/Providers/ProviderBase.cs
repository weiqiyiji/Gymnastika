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

        IEnumerable<T> Fetch(int startIndex, int number, Func<T, bool> predicate);
        
        void CreateOrUpdate(T entity);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        int Count(Func<T, bool> predicate);

        IEnumerable<T> All();

        IWorkContextScope GetContextScope();
    }

    public class ProviderBase<T> : IProvider<T>
    {
        IRepository<T> _repository { get; set; }
        
        IWorkEnvironment _environment { get; set; }
        
        public ProviderBase(IRepository<T> repository,IWorkEnvironment environment)
        {
            _repository = repository;
            _environment = environment;
        }

        public virtual IEnumerable<T> Fetch(int startIndex, int number)
        {
            return Fetch(startIndex, number, t => true);
        }

        public virtual IEnumerable<T> Fetch(int startIndex, int number,Func<T,bool> predicate)
        {
            return Fetch(predicate).Skip(startIndex).Take(number);
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
            return Fetch(null);
        }

        public IWorkContextScope GetContextScope()
        {
            return _environment.GetWorkContextScope();
        }

        //Modify!!
        public int Count(Func<T, bool> predicate)
        {
            if (predicate == null)
                predicate = t => true;
            return Fetch(t => true).Where(predicate).Count();   //Temporary
            //return _repository.Count((t) => predicate(t));
        }

        //Modify!!
        public virtual IEnumerable<T> Fetch(Func<T, bool> predicate)
        {


            if (predicate == null)
                return _repository.Fetch(t => true);
            else
                return _repository.Fetch(t => true).Where(predicate);   //Temporary
            //return _repository.Fetch(t => predicate(t));
        }
    }
}
