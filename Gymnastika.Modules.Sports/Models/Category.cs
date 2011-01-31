using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Gymnastika.Modules.Sports.Models
{
    public class Category
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ImageUri { get; set; }

        public ObservableCollection<Sport> Sports { get; set; }
    }
}
