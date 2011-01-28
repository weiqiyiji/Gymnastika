using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Gymnastika.Common;
using Microsoft.Practices.Unity;

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
            RegisterService();
            Run();
        }

        #endregion

        private void Run()
        {
            throw new NotImplementedException();
        }


        private void RegisterService()
        {
            throw new NotImplementedException();
        }

    }
}
