using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System.Windows;
using Gymnastika.Modules.Sports.Views;
using Gymnastika.Modules.Sports.Services;
using Gymnastika.Modules.Sports.ViewModels;

namespace Gymnastika.Modules.Sports
{
    public class SportsManagementModule : IModule
    {
        readonly private IRegionManager _regionManager;
        readonly private IUnityContainer _container;

        public SportsManagementModule(IUnityContainer container, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _container = container;
        }


        #region IModule Members

        public void Initialize()
        {
            RegisterDependencies();
        }

        #endregion


        #region RegisterDependencies

        private void RegisterDependencies()
        {
        }

        #endregion
    
    }
}
