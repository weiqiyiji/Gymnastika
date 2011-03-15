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
            if (string.IsNullOrEmpty(regionName))
                throw new ArgumentNullException("regionName");

            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentNullException("viewName");

            INavigationRegion region = _navigationManager.Regions[regionName];

            if (region == null)
            {
                throw new InvalidOperationException(
                    string.Format("Navigation Region [{0}] does not exist"));
            }

            NavigationDescriptor descriptor = region.ActiveNavigationViews[viewName];

            if (descriptor == null)
            {
                descriptor = region.Activate(viewName);
            }

            if (_previousDescriptor != null && _previousDescriptor.ViewName != descriptor.ViewName)
            {
                Presenter.ApplyTransition(_previousDescriptor.ViewResolver(), descriptor.ViewResolver());
            }
            
            _previousDescriptor = descriptor;
        }

        #endregion
    }
}