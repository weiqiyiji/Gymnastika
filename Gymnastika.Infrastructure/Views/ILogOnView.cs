using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Common;
using Gymnastika.ViewModels;

namespace Gymnastika.Views
{
    public interface ILogOnView
    {
        LogOnViewModel Model { get; set; }
        void Show();
    }
}
