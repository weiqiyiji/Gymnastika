using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public class FavoriteFoodProvider : IFavoriteFoodProvider
    {
        private readonly IRepository<FavoriteFood> _repository;

        public FavoriteFoodProvider(IRepository<FavoriteFood> repository)
        {
            _repository = repository;
        }

        #region IFavoriteFoodProvider Members

        public void Create(FavoriteFood favoriteFood)
        {
            _repository.Create(favoriteFood);
        }

        public void Update(FavoriteFood favoriteFood)
        {
            _repository.Update(favoriteFood);
        }

        public FavoriteFood Get(int userId)
        {
            return _repository.Get(f => f.User.Id == userId);
        }

        public void CreateOrUpdate(FavoriteFood favoriteFood)
        {
            _repository.CreateOrUpdate(favoriteFood);
        }

        #endregion
    }
}
