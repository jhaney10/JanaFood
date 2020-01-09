using JanaFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JanaFood.Services
{
    public interface IFoodData
    {
        IEnumerable<Food> GetAll();
        Food SaveFood(Food food);
        Food GetFood(int id);
        Food UpdateFood(Food food);
        string DeleteFood(int id);
        Order OrderFood(Order order);

    }
}
