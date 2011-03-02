using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Services
{
    public class CategoriesProvider : ICategoriesProvider
    {
        IRepository<SportsCategory> _repository;
        IWorkEnvironment _environment;

        public CategoriesProvider(IRepository<SportsCategory> repository, IWorkEnvironment environment)
        {
            _repository = repository;
            _environment = environment;
        }

        #region ICategoriesProvider Members

        public IEnumerable<SportsCategory> Fetch(Func<Models.SportsCategory, bool> predicate)
        {
            using (var scope = _environment.GetWorkContextScope())
            {
                return _repository.Fetch(t => predicate(t));
            }
        }

        #endregion
    }
}
