using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.ViewModels;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Modules.Sports.Services.Providers;
using Gymnastika.Services.Session;

namespace Gymnastika.Modules.Sports.Services.Factories
{
    public interface ISportsPlanViewModelFactory
    {
        ISportsPlanViewModel Create(SportsPlan plan);
    }

    public class SportsPlanViewModelFactory : ISportsPlanViewModelFactory
    {
        readonly ISportsPlanProvider _planProvider;
        readonly IPlanItemProvider _itemProvider;
        readonly ISportsPlanItemViewModelFactory _itemFactory;
        readonly ISportProvider _sportProvider;
        readonly ISessionManager _sessionManager;

        public SportsPlanViewModelFactory(ISessionManager sessionManager,ISportsPlanProvider planProvider,ISportProvider sportProvider,IPlanItemProvider itemProvider,ISportsPlanItemViewModelFactory itemFactory)
        {
            _planProvider = planProvider;
            _itemProvider = itemProvider;
            _itemFactory = itemFactory;
            _sportProvider = sportProvider;
            _sessionManager = sessionManager;
        }
        
        public ISportsPlanViewModel Create(SportsPlan plan)
        {
            return new SportsPlanViewModel(plan,_sessionManager,_sportProvider ,_planProvider, _itemProvider, _itemFactory);
        }
    }
}
