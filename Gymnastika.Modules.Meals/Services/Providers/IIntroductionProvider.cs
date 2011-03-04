using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public interface IIntroductionProvider
    {
        void Create(Introduction introduction);
        void Update(Introduction introduction);
    }
}
