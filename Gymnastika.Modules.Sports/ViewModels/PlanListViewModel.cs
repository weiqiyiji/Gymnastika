using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Sports.Services.Providers;
using Gymnastika.Modules.Sports.Models;
using System.Collections.ObjectModel;
using Gymnastika.Services.Session;
using Gymnastika.Modules.Sports.Extensions;
using Microsoft.Practices.Prism.Commands;
using Gymnastika.Modules.Sports.Services.Factories;
using Gymnastika.Services.Models;
using System.Collections.Specialized;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface IPlanListViewModel
    {
        DelegateCommand CreatePlanCommand { get; }

        User User { get; }

        ObservableCollection<SportsPlan> Plans { get; }
    }

    public class PlanListViewModel : NotificationObject, IPlanListViewModel
    {
        readonly ISportsPlanProvider _provider;
        readonly ISessionManager _sessionManager;
        readonly ISportsPlanViewModelFactory _planFactory;

        public PlanListViewModel(ISportsPlanProvider provider,ISessionManager sessionManager,ISportsPlanViewModelFactory planFactory)
        {
            _provider = provider;
            _sessionManager = sessionManager;
            _planFactory = planFactory;
            Plans.CollectionChanged += OnPlansChanged;
        }

        ObservableCollection<SportsPlan> _plans;
        public ObservableCollection<SportsPlan> Plans
        {
            get
            {
                if (_plans == null)
                {
                    _plans = LoadPlans().ToObservableCollection();
                }
                return _plans;
            }
        }

        IList<SportsPlan> LoadPlans()
        {
            using (_provider.GetContextScope())
            {
                return _provider.Fetch(t => (t.User.Id == User.Id)).ToList();
            }
        }

        DelegateCommand _createPlanCommand;
        public DelegateCommand CreatePlanCommand
        {
            get
            {
                if (_createPlanCommand == null)
                    _createPlanCommand = new DelegateCommand(CreatePlan);
                return _createPlanCommand;
            }
        }

        User _user;
        public User User
        {
            get { return _sessionManager.GetCurrentSession().AssociatedUser; }
        }

        void CreatePlan()
        {
            Plans.Add(new SportsPlan() { User =  User});
        }

        void OnPlansChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }
    }    
}
