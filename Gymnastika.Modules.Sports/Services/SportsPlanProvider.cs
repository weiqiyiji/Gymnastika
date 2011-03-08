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

        IRepository<SportsPlanItem> _itemsRepository;
        IRepository<SportsPlan> _planRepository;
        IWorkEnvironment _environment;

        public SportsPlanProvider(IRepository<SportsPlan> planRepository,IRepository<SportsPlanItem> itemsRepository, IWorkEnvironment environment)
            :base(planRepository,environment)
        {
            _itemsRepository = itemsRepository;
            _planRepository = planRepository;
            _environment = environment;
        }

        void CreateOrUpdateItems(SportsPlan plan)
        {
            var items = plan.SportsPlanItems;
            foreach (SportsPlanItem item in items)
            {
                item.SportsPlanId = plan.Id;
                _itemsRepository.CreateOrUpdate(item);
            }
        }

        public override void CreateOrUpdate(SportsPlan entity)
        {
            if(entity.Id  == 0)
            {
                var items = entity.SportsPlanItems ?? new List<SportsPlanItem>();
                entity.SportsPlanItems = new List<SportsPlanItem>();
                _planRepository.Create(entity);
                entity.SportsPlanItems = items;
            }
            CreateOrUpdateItems(entity);
        }
    }
}
