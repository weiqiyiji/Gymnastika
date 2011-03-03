using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Services
{
    public class XmlCatagoryProvider : ICategoriesProvider
    {
        #region ICategoriesProvider Members

        public IEnumerable<Models.SportsCategory> Fetch(Func<Models.SportsCategory, bool> predicate)
        {
            yield return new SportsCategory()
            {
                Name = "有氧运动",
                ImageUri = "/Data/cat1.jpg",
                Note = "有氧运动就是有氧的运动",
                Sports = new List<Sport>()
                {
                    new Sport()
                    {
                        Name = "慢跑",
                        Brief = "慢跑是一种常用的减肥方法，他通过消耗能量达到减肥的目的",
                    },
                    new Sport()
                    {
                        Name = "散步",
                        Brief = "散步是一种怡情的减肥策略",
                    },
                },

            };
            yield return new SportsCategory()
            {
                Name = "球类运动",
                ImageUri = "/Data/cat1.jpg",
                Note = "和球有关的运动",
                Sports = new List<Sport>()
                {
                    new Sport()
                    {
                        Name = "足球",
                        Brief = "足球是一种用蹄子开展的运动",
                    },
                    new Sport()
                    {
                        Name = "羽毛球",
                        Brief = "与足球相反，羽毛球用的是爪子",
                    },
                }
            };
            yield return new SportsCategory()
            {
                Name = "绝食运动",
                ImageUri = "/Data/cat1.jpg",
                Note = "不吃东西",
                Sports = new List<Sport>()
                {
                    new Sport()
                    {
                        Name = "不吃面包",
                        Brief = "其实你可以吃蛋糕的",
                    }
                }
            };
        }

        #endregion
    }
}
