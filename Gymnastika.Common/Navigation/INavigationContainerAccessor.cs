using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluidKit.Controls;

namespace Gymnastika.Common.Navigation
{
    public interface INavigationContainerAccessor
    {
        TransitionPresenter Container { get; set; }
        bool IsContainerReady { get; }
        event EventHandler ContainerReady;
    }

    public class NavigationContainerAccessor : INavigationContainerAccessor
    {
        private TransitionPresenter _container;

        #region INavigationContainerAccessor Members

        public bool IsContainerReady
        {
            get { return Container != null; }
        }

        public TransitionPresenter Container
        {
            get
            {
                return _container;
            }
            set
            {
                if(_container == null && value != null)
                {
                    _container = value;
                    OnContainerReady();
                }
            }
        }

        private void OnContainerReady()
        {
            if(ContainerReady != null)
                ContainerReady(this, EventArgs.Empty);
        }

        public event EventHandler ContainerReady;

        #endregion
    }


}
