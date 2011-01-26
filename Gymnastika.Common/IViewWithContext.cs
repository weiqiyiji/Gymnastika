using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Common
{
    public interface IViewWithContext
    {
        object Model { get; set; }
    }
}
