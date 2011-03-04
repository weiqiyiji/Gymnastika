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

        public IEnumerable<T> Fetch(Func<T, bool> predicate)
        {
            using (var scope = _environment.GetWorkContextScope())
            {
                return _repository.Fetch(t => predicate(t));
            }
        }

        public IEnumerable<T> Fetch(int startIndex, int number)
        {
            using (var scope = _environment.GetWorkContextScope())
            {
                return _repository.Fetch(t => true).Take(number).Skip(startIndex);
            }
        }

        public void CreateOrUpdate(T entity)
        {
            using (var scope = _environment.GetWorkContextScope())
            {
                _repository.CreateOrUpdate(entity);
            }
        }
        public void Create(T entity)
        {
            using (var scope = _environment.GetWorkContextScope())
            {
                _repository.Create(entity);
            }
        }

        public void Update(T entity)
        {
            using (var scope = _environment.GetWorkContextScope())
            {
                _repository.Update(entity);
            }
        }
        public void Delete(T entity)
        {
            using (var scope = _environment.GetWorkContextScope())
            {
                _repository.Delete(entity);
            }
        }
    }
}
