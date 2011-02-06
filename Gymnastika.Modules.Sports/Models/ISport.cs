using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Gymnastika.Modules.Sports.Interface
{
    /// <summary>
    /// Describe a kind of sport
    /// </summary>
    public interface ISport
    {
        string Name { get; set; }
        ImageSource Image { get; set; }
    }
}
