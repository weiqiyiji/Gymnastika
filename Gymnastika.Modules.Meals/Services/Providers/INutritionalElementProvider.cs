using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public interface INutritionalElementProvider
    {
        void Create(NutritionalElement nutritionalElement);
        void Update(NutritionalElement nutritionalElement);
    }
}
