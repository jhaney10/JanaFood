using JanaFood.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JanaFood.ViewModels
{
    public class ManageOrderViewModel
    {
        public IEnumerable<Order> Orders { get; set; }

    }
}
