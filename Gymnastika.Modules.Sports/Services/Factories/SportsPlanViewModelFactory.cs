using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.ViewModels;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Modules.Sports.Services.Providers;
using Gymnastika.Services.Session;
using Microsoft.Practices.Prism.Events;

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
        readonly IEventAggregator _eventAggregator;

        public SportsPlanViewModelFactory(ISessionManager sessionManager,ISportsPlanProvider planProvider,ISportProvider sportProvider,IPlanItemProvider itemProvider,ISportsPlanItemViewModelFactory itemFactory,IEventAggregator eventAggregator)
        {
            _planProvider = planProvider;
            _itemProvider = itemProvider;
            _itemFactory = itemFactory;
            _sportProvider = sportProvider;
            _sessionManager = sessionManager;
            _eventAggregator = eventAggregator;
        }
        
        public ISportsPlanViewModel Create(SportsPlan plan)
        {
            return new SportsPlanViewModel(plan,_sessionManager,_sportProvider ,_planProvider, _itemProvider, _itemFactory,_eventAggregator);
        }
    }
}
