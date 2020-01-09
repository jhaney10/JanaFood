using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JanaFood.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public AppUser Customer { get; set; }
        public Food CustomerOrder { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string DeliveryAddress { get; set; }

    }
}
