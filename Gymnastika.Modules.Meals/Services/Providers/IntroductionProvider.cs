using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;
using Gymnastika.Data;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public class IntroductionProvider : IIntroductionProvider
    {
        private readonly IRepository<Introduction> _repository;

        public IntroductionProvider(IRepository<Introduction> repository)
        {
            _repository = repository;
        }

        #region IIntroductionProvider Members

        public void Create(Introduction introduction)
        {
            _repository.Create(introduction);
        }

        public void Update(Introduction introduction)
        {
            _repository.Update(introduction);
        }

        public IEnumerable<Introduction> GetIntroductions(Food food)
        {
            return _repository.Fetch(i => i.Food == food);
        }

        #endregion
    }
}
