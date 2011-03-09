using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public interface IFavoriteFoodProvider
    {
        void Create(FavoriteFood favoriteFood);
        void Update(FavoriteFood favoriteFood);
        FavoriteFood Get(int userId);
        void CreateOrUpdate(FavoriteFood favoriteFood);
    }
}
