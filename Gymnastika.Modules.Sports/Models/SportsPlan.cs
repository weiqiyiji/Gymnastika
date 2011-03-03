﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.Prism.ViewModel;
using System.Collections.ObjectModel;

namespace Gymnastika.Modules.Sports.Models
{
    public class SportsPlan
    {
        public virtual int Id { set; get; }

        public virtual DateTime Time { get; set; }

        public virtual int Score { get; set; }

        public virtual IList<SportsPlanItem> SportsPlanItems { get; set; }
    }
}
