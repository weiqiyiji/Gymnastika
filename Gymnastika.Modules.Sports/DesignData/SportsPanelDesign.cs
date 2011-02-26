using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.ViewModels;
using System.Collections.ObjectModel;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.DesignData
{
    public class SportsPanelDesign
    {
        public SportsPanelDesign()
        {
            Sports = new ObservableCollection<Sport>();
        }
        public ObservableCollection<Sport> Sports
        {
            get; set;
        }

    }
}
