using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Data;

namespace Gymnastika.Modules.Sports.Services
{
    public class SportsPlanProvider : ProviderBase<SportsPlan>, ISportsPlanProvider
    {


        IRepository<SportsPlan> _repository;
        IWorkEnvironment _environment;

        public SportsPlanProvider(IRepository<SportsPlan> repository, IWorkEnvironment environment)
            :base(repository,environment)
        {
            _repository = repository;
            _environment = environment;
        }

    }
}
