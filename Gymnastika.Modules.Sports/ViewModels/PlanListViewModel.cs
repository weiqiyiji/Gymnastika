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
using Gymnastika.Common.Extensions;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface IPlanListViewModel
    {
        DelegateCommand CreatePlanCommand { get; }

        User User { get; }

        ObservableCollection<ISportsPlanViewModel> ViewModels { get; }
    }

    public class PlanListViewModel : NotificationObject, IPlanListViewModel
    {
        readonly ISportsPlanProvider _planProvider;
        readonly IPlanItemProvider _itemProvider;
        readonly ISessionManager _sessionManager;
        readonly ISportsPlanViewModelFactory _planFactory;

        public PlanListViewModel(ISportsPlanProvider planProvider,IPlanItemProvider itemProvider,ISessionManager sessionManager,ISportsPlanViewModelFactory planFactory)
        {
            _planProvider = planProvider;
            _itemProvider = itemProvider;
            _sessionManager = sessionManager;
            _planFactory = planFactory;
            ViewModels.CollectionChanged += OnPlansChanged;
            ViewModels.AddRange(CreateViewModels(Plans));
        }

        ObservableCollection<ISportsPlanViewModel> _viewModels = new ObservableCollection<ISportsPlanViewModel>();
        public ObservableCollection<ISportsPlanViewModel> ViewModels
        {
            get 
            {
                return _viewModels; 
            }
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
            using (_planProvider.GetContextScope())
            {
                IList<SportsPlan> plans   =_planProvider.Fetch(t => (t.User == User)).ToList();
                //Avoid Lazy Loading
                foreach (SportsPlan plan in plans)
                {
                    plan.SportsPlanItems = plan.SportsPlanItems.ToList();
                    plan.User = User;
                }
                return plans;
            }
        }


        public User User
        {
            get { return _sessionManager.GetCurrentSession().AssociatedUser; }
        }


        ObservableCollection<ISportsPlanViewModel> CreateViewModels(IEnumerable<SportsPlan> plans)
        {
            ObservableCollection<ISportsPlanViewModel> viewmodels = new ObservableCollection<ISportsPlanViewModel>();
            foreach (var plan in plans)
            {
                viewmodels.Add(CreateViewModel(plan));
            }
            return viewmodels;
        }

        ISportsPlanViewModel CreateViewModel(SportsPlan plan)
        {
            return _planFactory.Create(plan);
        }

        void OnPlansChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<SportsPlan> plans = sender as ObservableCollection<SportsPlan>;
            var newModels = e.NewItems;
            var oldModels = e.OldItems;
            if(newModels!=null)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach(ISportsPlanViewModel plan in oldModels)
                        {
                            OnReleasePlan(plan);
                        }
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        foreach (ISportsPlanViewModel plan in oldModels)
                        {
                            OnReleasePlan(plan);
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        void OnReleasePlan(ISportsPlanViewModel viewmodel)
        {
            Plans.Remove(viewmodel.SportsPlan);
            
        }

        void DeletePlanFromRepository(SportsPlan plan)
        {
            if (plan.Id != 0)
            {
                using (_planProvider.GetContextScope())
                {

                    foreach (SportsPlanItem item in plan.SportsPlanItems)
                    {
                        _itemProvider.Delete(item);
                        plan.SportsPlanItems.Remove(item);
                    }
                    _planProvider.Delete(plan);
                }
            }
            plan.Id = 0;
        }

        void CreateOrUpdatePlanToRepository(SportsPlan plan)
        {
            using (_planProvider.GetContextScope())
            {
                _planProvider.CreateOrUpdate(plan);
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

       void CreatePlan()
       {
           SportsPlan newPlan = new SportsPlan() { User = this.User };
           CreateOrUpdatePlanToRepository(newPlan);

           ViewModels.Add(CreateViewModel(newPlan));
       }
    }
}
