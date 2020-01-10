using JanaFood.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JanaFood.Models
{
    public class InMemoryData : IFoodData
    {
        private List<Food> _fooddata;

        public InMemoryData()
        {
            _fooddata = new List<Food>
            {
                new Food{Id= 1, Name = "Moi Moi", Price= 1500M,},
                new Food{Id = 2, Name = "Jollof Rice with Chicken", 
                    Description="Includes One portion of Grilled Chicken", Price = 2500M}
            };
        }

        public string DeleteFood(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Food> GetAll()
        {
            return _fooddata.OrderBy(x => x.Name);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            throw new NotImplementedException();
        }

        public Food GetFood(int id)
        {
            throw new NotImplementedException();
        }

        public Order GetOrder(int id)
        {
            throw new NotImplementedException();
        }

        public Order OrderFood(Order order)
        {
            throw new NotImplementedException();
        }

        public Food SaveFood(Food food)
        {
            throw new NotImplementedException();
        }

        public Food UpdateFood(Food food)
        {
            throw new NotImplementedException();
        }

        public string UpdateStatus(string status, int id)
        {
            throw new NotImplementedException();
        }
    }
}
