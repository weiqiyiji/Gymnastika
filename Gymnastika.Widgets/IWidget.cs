using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Widgets
{
    public interface IWidget
    {
        bool IsActive { get; set; }
        event EventHandler IsActiveChanged;
        void Initialize();
    }
}
