using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Data;

namespace Gymnastika.Modules.Sports.Data
{
    public class CategoryImporter : IDataImporter<SportsCategory>
    {
        IRepository<SportsCategory> _repository;
        IWorkEnvironment _environment;
        public CategoryImporter(IRepository<SportsCategory> repository, IWorkEnvironment environment)
        {
            _repository = repository;
            _environment = environment;
        }
        #region IDataImporter<SportsCategory> Members

        public bool NeedImport()
        {
            using (var scope = _environment.GetWorkContextScope())
            {
                return _repository.Count(t => true) == 0;
            }
        }

        public void ImportData(IDataSource<SportsCategory> source)
        {
            throw new NotImplementedException();

            //using (var scope = _environment.GetWorkContextScope())
            //{
            //    SportsCategory category = new SportsCategory();
            //    _repository.Create(category);
            //    category.ImageUri = 
            //}
        }

        #endregion
    }
}
