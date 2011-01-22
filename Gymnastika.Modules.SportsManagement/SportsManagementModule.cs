using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Gymnastika.Desktop.Core;

namespace Gymnastika.Modules.SportsManagement
{
    public class SportsManagementModule : IModule
    {
        readonly private IRegionViewRegistry _regionViewRegistry;

        public SportsManagementModule(IRegionViewRegistry regionViewRegistry)
        {
            _regionViewRegistry = regionViewRegistry;
        }

        #region IModule Members

        public void Initialize()
        {
#if (DEBUG)
            if (_regionViewRegistry == null)
                throw new Exception("SportsModule:Initialize:RegionViewRegistry is null");
#endif
            _regionViewRegistry.RegisterViewWithRegion(RegionNames.MainRegion, typeof(SportsManagementView));
        }

        #endregion
    }
}
