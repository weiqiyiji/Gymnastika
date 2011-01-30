using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.ProjectResources.Properties;

namespace Gymnastika.ViewModels
{
    public class ShellViewModel : NotificationObject
    {
        public string Title
        {
            get { return Resources.AppTitle; }
        }
    }
}
