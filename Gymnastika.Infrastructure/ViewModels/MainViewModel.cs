using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Controllers;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Views;
using Microsoft.Practices.Unity;

namespace Gymnastika.ViewModels
{
    public class MainViewModel : NotificationObject, INavigationAware
    {
        private readonly IUnityContainer _container;

        public MainViewModel(
            IUnityContainer container)
        {
            _container = container;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Initialize();
            LoadModules();
        }

        private void Initialize()
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void LoadModules()
        {
            IModuleCatalog moduleCatelog = _container.Resolve<IModuleCatalog>();
            IModuleManager moduleManager = _container.Resolve<IModuleManager>();

            foreach (var moduleInfo in moduleCatelog.Modules)
            {
                moduleManager.LoadModule(moduleInfo.ModuleName);
            }
        }

    }
}
