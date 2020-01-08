using JanaFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JanaFood.Services
{
    public class SqlFoodData : IFoodData
    {
        private ApplicationDbContext _context;

        public SqlFoodData(ApplicationDbContext context)
        {
            _context = context;
        }

        public string DeleteFood(int id)
        {
            var foodDetails = _context.Foods.FirstOrDefault(x => x.Id == id);
            if (foodDetails != null)
            {
                _context.Foods.Remove(foodDetails);
                _context.SaveChanges();
                string message = "Entry has been deleted";
                return message;
            }
            else
            {
                string message = "Invalid Request";
                return message;
            }
            
        }

        public IEnumerable<Food> GetAll()
        {
            return _context.Foods.OrderBy(x => x.Name);
        }

        public Food GetFood(int id)
        {
            var foodDetails = _context.Foods.FirstOrDefault(x => x.Id == id);
            return foodDetails;
        }

        public Food SaveFood(Food food)
        {
            _context.Foods.Add(food);
            _context.SaveChanges();
            return food;
        }

        public Food UpdateFood(Food food)
        {
            Food foodEntry = _context.Foods.FirstOrDefault(x => x.Id == food.Id);
            if (foodEntry != null)
            {
                foodEntry.Name = food.Name;
                foodEntry.Description = food.Description;
                foodEntry.Price = food.Price;
                foodEntry.Picture = food.Picture;
                _context.SaveChanges();
            }

            return foodEntry;
        }
    }
}
