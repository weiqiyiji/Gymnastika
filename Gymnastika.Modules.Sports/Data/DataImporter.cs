using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Data
{
    public interface IDataImporter<T>
    {
        bool NeedImport();
        void ImportData(IEnumerable<T> source);
    }

    public class CategoryDataImporter : IDataImporter<SportsCategory>
    {
        IWorkEnvironment _environment;
        IRepository<SportsCategory> _repository;

        public CategoryDataImporter(IRepository<SportsCategory> repository, IWorkEnvironment environment)
        {
            _environment = environment;
            _repository = repository;
        }

        public void ImportData(IEnumerable<SportsCategory> source)
        {
            using(var scope = _environment.GetWorkContextScope())
            {
                foreach(SportsCategory entity in source)
                {
                    _repository.Create(entity);
                }
            }
        }

        public bool NeedImport()
        {
            using(var scope = _environment.GetWorkContextScope())
            {
                return _repository.Count(testc => true) != 0;
            }
        }
    }
}
