using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;
using Microsoft.Practices.Prism.Events;

namespace Gymnastika.Modules.Meals.Events
{
    public class SelectCategoryEvent : CompositePresentationEvent<Category>
    {
    }
}
