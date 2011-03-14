using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Gymnastika.Common.Navigation
{
    public interface INavigationManager
    {
        void AddRegionIfMissing(string regionName, string header);
        void RemoveRegion(string regionName);

        INavigationRegionCollection Regions { get; }
    }
}
