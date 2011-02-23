using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Services
{
    public class CategoriesProvider : Provider<SportsCategory> , ICategoriesProvider
    {
        IRepository<SportsCategory> _repository;
        IWorkEnvironment _environment;

        public CategoriesProvider(IRepository<SportsCategory> repository, IWorkEnvironment environment)
            :base(repository,environment)
        {

        }

    }
}
