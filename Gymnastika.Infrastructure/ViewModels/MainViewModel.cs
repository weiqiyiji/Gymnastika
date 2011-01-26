using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Views;

namespace Gymnastika.ViewModels
{
    public class MainViewModel : NotificationObject
    {
        public IMainView View { get; set; }

        public MainViewModel(IMainView mainView)
        {
            View = mainView;
            View.Model = this;
        }
    }
}
