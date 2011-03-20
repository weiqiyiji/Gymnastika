using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluidKit.Controls;

namespace Gymnastika.Common.Navigation
{
    public class NavigationService : INavigationService
    {
        private INavigationManager _navigationManager;
        private INavigationContainerAccessor _containerAccessor;
        private NavigationDescriptor _previousDescriptor;

        public NavigationService(INavigationManager navigationManager, INavigationContainerAccessor containerAccessor)
        {
            _navigationManager = navigationManager;
            _containerAccessor = containerAccessor;
        }

        #region INavigationService Members

        public TransitionPresenter Presenter
        {
            get { return _containerAccessor.Container; }
        }

        public void RequestNavigate(string regionName, string viewName)
        {
            RequestNavigate(regionName, viewName, null);
        }

        private bool _isInTransition;
        private NavigationDescriptor _targetDescriptor;
        private ViewState _targetState;

        public void RequestNavigate(string regionName, string viewName, string stateName)
        {
            if (_isInTransition) return;

            if (string.IsNullOrEmpty(regionName))
                throw new ArgumentNullException("regionName");

            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentNullException("viewName");

            INavigationRegion region = EnsureRegion(regionName);
            NavigationDescriptor descriptor = ActivateView(viewName, region);
            ViewState viewState = EnsureState(regionName, viewName, stateName, descriptor);

            OnNavigationStart(_previousDescriptor, descriptor, viewState);

            Presenter.TransitionCompleted -= OnTransitionCompleted;

            if (_previousDescriptor != null && _previousDescriptor.ViewName != descriptor.ViewName)
            {
                _targetDescriptor = descriptor;
                _targetState = viewState;
                Presenter.TransitionCompleted += OnTransitionCompleted;
                Presenter.ApplyTransition(_previousDescriptor.ViewResolver(), descriptor.ViewResolver());
            }
        }

        private void OnTransitionCompleted(object sender, EventArgs e)
        {
            OnNavigationCompleted(_previousDescriptor, _targetDescriptor, _targetState);

            if (_targetState != null)
            {
                NotifyStateChanging(_targetDescriptor, _targetState);
            }

            _previousDescriptor = _targetDescriptor;
        }

        private NavigationDescriptor ActivateView(string viewName, INavigationRegion region)
        {
            NavigationDescriptor descriptor = region.ActiveNavigationViews[viewName];

            if (descriptor == null)
            {
                descriptor = region.Activate(viewName);
            }

            return descriptor;
        }

        private ViewState EnsureState(
            string regionName, string viewName, string stateName, NavigationDescriptor descriptor)
        {
            ViewState viewState = null;

            if (!string.IsNullOrEmpty(stateName))
            {
                if (descriptor.States == null ||
                    (viewState = descriptor.States.SingleOrDefault(x => x.Name == stateName)) == null)
                {
                    throw new InvalidOperationException(
                        string.Format("{0}_{1}_{2} does not exist", regionName, viewName, stateName));
                }
            }

            return viewState;
        }

        private INavigationRegion EnsureRegion(string regionName)
        {
            INavigationRegion region = _navigationManager.Regions[regionName];

            if (region == null)
            {
                throw new InvalidOperationException(
                    string.Format("Navigation Region [{0}] does not exist"));
            }

            return region;
        }
            
        public event NavigationHandler NavigationStart;

        public event NavigationHandler NavigationCompleted;

        private void NotifyStateChanging(NavigationDescriptor descriptor, ViewState state)
        {
            if(descriptor.StateChanging != null)
                descriptor.StateChanging(state);
        }

        private void OnNavigationStart(NavigationDescriptor sourceDescriptor, NavigationDescriptor targetDescriptor, ViewState targetState)
        {
            if (NavigationStart != null)
                NavigationStart(this,
                    new NavigationEventArgs(sourceDescriptor, targetDescriptor) { TargetState = targetState });
        }

        private void OnNavigationCompleted(NavigationDescriptor sourceDescriptor, NavigationDescriptor targetDescriptor, ViewState targetState)
        {
            if (NavigationCompleted != null)
                NavigationCompleted(this,
                    new NavigationEventArgs(sourceDescriptor, targetDescriptor) { TargetState = targetState });
        }

        #endregion
    }
}