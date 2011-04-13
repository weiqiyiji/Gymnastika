using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Sports.Events;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Modules.Sports.Communication.Services;
using Gymnastika.Sync.Communication.Client;
using Gymnastika.Modules.Sports.Services.Providers;
using Gymnastika.Modules.Sports.Facilities;
using System.Diagnostics;
using System.ComponentModel;
using Gymnastika.Sync.Communication;
using Gymnastika.Services.Session;
using Gymnastika.Modules.Sports.Communication.Tasks;

namespace Gymnastika.Modules.Sports.Communication
{
    public enum CommunicationState { Busy, Idle };

    public class CommunicationStateChangedArgs : EventArgs
    {
        public CommunicationState State { get; set; }
    }

    public class CommunicationManager
    {
        ConnectionStore _connectionStore;
        ISportsPlanProvider _sportsPlanProvider;
        ISportProvider _sportProvider;
        CommunicationService _communicationService;
        IEventAggregator _aggregator;
        ISessionManager _sessionManager;
        IPlanItemProvider _itemProvider;

        int _sendingPlanCount = 0;
        public int SendingPlansCount
        {
            get { return _sendingPlanCount; }
            private set 
            {
                _sendingPlanCount = value;
                if (_sendingPlanCount != value)
                {
                    _sendingPlanCount = value;
                    UpdateState();
                }
            }
        }

        void UpdateState()
        {
            State = (SendingPlansCount == 0 && ReceivingPlansCount == 0) ? CommunicationState.Idle : CommunicationState.Busy;
        }

        int _receivingPlansCount = 0;
        public int ReceivingPlansCount
        {
            get { return _receivingPlansCount; }
            private set
            {
                if (_receivingPlansCount != value)
                {
                    _receivingPlansCount = value;
                    UpdateState();
                }
            }
        }

        public event EventHandler<CommunicationStateChangedArgs> CommunicationStateChanged = delegate { };

        CommunicationState _state = CommunicationState.Idle;
        public CommunicationState State
        {
            get { return _state; }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    if (CommunicationStateChanged != null)
                        CommunicationStateChanged(this, new CommunicationStateChangedArgs() { State = _state });
                }
            }
        }

        public CommunicationManager
            (IEventAggregator aggregator,
            CommunicationService communicationService,
            ISessionManager sessionManager,
            ConnectionStore connectionStore,
            ISportsPlanProvider sportsPlanProvider,
            IPlanItemProvider itemProvider,
            ISportProvider sportProvider)
        {
            _itemProvider = itemProvider;
            _sessionManager = sessionManager;
            _aggregator = aggregator;
            _communicationService = communicationService;
            _connectionStore = connectionStore;
            _sportsPlanProvider = sportsPlanProvider;
            _sportProvider = sportProvider;
            _aggregator.GetEvent<SportsPlanCreatedOrModifiedEvent>().Subscribe(OnPlanCreatedOrModified);
            connectionStore.OnConnectionEstablished += OnConnectionEstablished;
        }


        void OnPlanCreatedOrModified(SportsPlan plan)
        {
            SendPlan(plan);
        }

        void SendPlan(SportsPlan plan)
        {

            if (plan.Id == 0)
                throw new Exception("NotSavedToDatabase");

            if (plan.SynchronizedToServer)
                return;

            if (_connectionStore.IsConnectionEstablished)
            {

                int connectedId = _connectionStore.ConnectionId;
                _communicationService.SendPlan(plan, connectedId, OnPlanSychronizeFinished);
            }
        }

        void OnPlanSychronizeFinished(ResponseMessage message, SportsPlan plan)
        {
            
            if (!message.HasError)
            {
                using (_sportsPlanProvider.GetContextScope())
                {
                    SportsPlan planInRepository = _sportsPlanProvider.Get(plan.Id);
                    planInRepository.SynchronizedToServer = true;
                    _sportsPlanProvider.CreateOrUpdate(planInRepository);
                }
            }
            else
            {
                Debug.WriteLine("CommunicationManager: Send Plan Failed");
            }
            --SendingPlansCount;
        }

        void OnConnectionEstablished(object sender, EventArgs args)
        {
            ConnectionStore connectionStore = sender as ConnectionStore;
            if (connectionStore != null)
            {
                AsychronousLoadHelper.AsychronousCall(SendMessage);
                UpdatePlans();
            }
        }

        void SendMessage()
        {
            lock (this)
            {
                State = CommunicationState.Busy;
                var plans = LoadUnsychronizedPlans();
                SendingPlansCount += plans.Count;
                foreach (var plan in plans)
                    SendPlan(plan);
            }
        }

        bool LaterThan(SportsPlan plan, DateTime date)
        {
            return plan.Year > date.Year || plan.Month > date.Month || plan.Day > date.Day;
        }

        IList<SportsPlan> LoadUnsychronizedPlans()
        {
            IList<SportsPlan> plans;
            using (_sportsPlanProvider.GetContextScope())
            {
                DateTime now = DateTime.Now.AddMinutes(1);
                plans = _sportsPlanProvider.FetchUnSychronizedItems().Where(t => LaterThan(t, now)).ToList();
                foreach (var plan in plans)
                {
                    plan.SportsPlanItems = plan.SportsPlanItems.ToList();
                    foreach (var item in plan.SportsPlanItems)
                    {
                        item.Sport = _sportProvider.Get(item.Sport.Id);
                    }
                }
            }
            return plans;
        }

        public void UpdatePlans()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(BeginUpdatePlans);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UpdateCompleted);
            worker.RunWorkerAsync();
        }

        void UpdateCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IList<SportsPlan> plans = (e.Result as IList<SportsPlan>);
            using (_sportsPlanProvider.GetContextScope())
            {
                plans = plans.Select(s => _sportsPlanProvider.GetWithOutDelay(s.Id)).ToList();
            }

            SportsPlanCreatedOrModifiedEvent evt = _aggregator.GetEvent<SportsPlanCreatedOrModifiedEvent>();
            foreach (var plan in plans)
            {
                evt.Publish(plan);
            }
        }

        void BeginUpdatePlans(object sender, DoWorkEventArgs e)
        {
            ++ReceivingPlansCount;
            var items = _communicationService.GetCompletedTasks(_sessionManager.GetCurrentSession().AssociatedUser.Id);
            Dictionary<int, SportsPlan> plans = new Dictionary<int, SportsPlan>();
            
            if (items != null && items.Count != 0)
            {
                using (_itemProvider.GetContextScope())
                {
                    foreach (var item in items)
                    {
                        var planItem = _itemProvider.Get(item.Id);
                        planItem.Completed = true;
                        SportsPlan plan;
                        int planId = planItem.SportsPlan.Id;
                        if(!plans.TryGetValue(planId,out plan))
                        {
                            plan = _sportsPlanProvider.GetWithOutDelay(planId);
                            plans.Add(plan.Id, plan);
                        }
                    }
                }
            }
            --ReceivingPlansCount;
            e.Result = plans.Select(t => t.Value).ToList();
        }

    }

}