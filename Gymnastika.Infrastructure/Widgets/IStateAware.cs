using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Widgets
{
    public interface IStateAware
    {
        void OnResume();
        void OnStop();
    }
}
