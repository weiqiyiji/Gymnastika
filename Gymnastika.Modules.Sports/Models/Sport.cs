using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Media;

namespace Gymnastika.Modules.Sports.Models
{
    public class Sport
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public string SmallImageUri { get; set; }

        public string LargeImageUri { get; set; }

        public string BriefIntroduction { get; set; }

        public string DetailIntroduction { get; set; }

        public int Calorie { get; set; }

        public string Catalog { get; set; }
    }
}
