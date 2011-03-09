using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Services
{
    public class CategoriesProvider : ProviderBase<SportsCategory> , ICategoriesProvider
    {
        IRepository<Sport> _sportRepository;
        IRepository<SportsCategory> _categoryRepository;
        IWorkEnvironment _environment;

        public CategoriesProvider(IRepository<SportsCategory> categoryRepository, IRepository<Sport> sportRepository,IWorkEnvironment environment)
            :base(categoryRepository,environment)
        {
            _sportRepository = sportRepository;
            _categoryRepository = categoryRepository;
            _environment = environment;
        }
        //public override void CreateOrUpdate(SportsCategory entity)
        //{
        //    base.CreateOrUpdate(entity);
        //    foreach (Sport sport in entity.Sports)
        //    {
        //        sport.SportsCategories = sport.SportsCategories ?? new List<SportsCategory>();
        //        if (!sport.SportsCategories.Contains(entity))
        //        {
        //            sport.SportsCategories.Add(entity);
        //            _sportRepository.CreateOrUpdate(sport);
        //        }
        //    }
        //}
    }
}
