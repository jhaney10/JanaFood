using JanaFood.Models;
using Microsoft.EntityFrameworkCore;
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

        public Order OrderFood(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders.OrderBy(x => x.OrderDate).Include(f => f.Customer)
                .Include(f => f.CustomerOrder).ToList();
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
        public string UpdateStatus(string status, int id)
        {
            Order order = _context.Orders.FirstOrDefault(x => x.OrderId == id);
            if (order != null)
            {
                order.OrderStatus = status;
                _context.SaveChanges();
                var result = "Status Update Successful";
                return result;
            }
            else
            {
                var result = "Status Update Not Successful";
                return result;
            }
        }

        public Order GetOrder(int id)
        {
            return _context.Orders.Include(x => x.Customer).Include(x => x.CustomerOrder)
                .FirstOrDefault(c => c.OrderId == id);
        }
    }
}
