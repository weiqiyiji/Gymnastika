using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Data;
using Gymnastika.Modules.Sports.DataImport.Sources;

namespace Gymnastika.Modules.Sports.DataImport.Importers
{
    public class CategoryImporter : IDataImporter
    {
        IRepository<SportsCategory> _categoryRepository;
        IRepository<Sport> _sportRepository;
        IWorkEnvironment _environment;
        IDataSource<SportsCategory> _source;
        
        public CategoryImporter(IDataSource<SportsCategory> source, IRepository<SportsCategory> repository, IRepository<Sport> sportRepository, IWorkEnvironment environment)
        {
            _categoryRepository = repository;
            _sportRepository = sportRepository;
            _environment = environment;
            _source = source;
        }

        #region IDataImporter<SportsCategory> Members

        public bool NeedImport()
        {
            using (var scope = _environment.GetWorkContextScope())
            {
                return _categoryRepository.Count(t => true) == 0;
            }
        }

        public void ImportData()
        {
            using (var scope = _environment.GetWorkContextScope())
            {
                foreach (SportsCategory category in _source.GetData())
                {
                    //Get sports
                    var sports = category.Sports;
                    
                    //Clear
                    category.Sports = new List<Sport>();
                    
                    //Create
                    _categoryRepository.Create(category);
                    _categoryRepository.Flush();

                    //Add sports
                    foreach(Sport sport in sports)
                    {
                        sport.SportsCategories = sport.SportsCategories ?? new List<SportsCategory>();
                        _sportRepository.Create(sport);
                        
                        //Build Relations
                        category.Sports.Add(sport);
                        sport.SportsCategories.Add(category);
                        
                    }

                    //Reset
                    category.Sports = sports;
                }
            }
        }

        #endregion
    }
}
