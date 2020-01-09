using JanaFood.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JanaFood.ViewModels
{
    public class OrderFoodViewModel
    {
        public AppUser Customer { get; set; }
        public Food CustomerOrder { get; set; }
        public DateTime OrderDate { get; set; }

        [Required]
        [Display(Name = "Enter your preferred delivery address")]
        public string DeliveryAddress { get; set; }

    }
}
