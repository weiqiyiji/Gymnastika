using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluidKit.Controls;

namespace Gymnastika.Common.Navigation
{
    public interface INavigationService
    {
        void RequestNavigate(string regionName, string viewName);
    }
}
