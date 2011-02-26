using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;

namespace Gymnastika.Modules.Sports.Services
{
    public class ProviderBase<T>
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
    }
}
