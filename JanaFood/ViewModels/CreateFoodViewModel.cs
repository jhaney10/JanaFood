using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JanaFood.ViewModels
{
    public class CreateFoodViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter the Food Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage ="Please enter the Food Price")]
        public decimal Price { get; set; }
        public IFormFile Photo { get; set; }
    }
}
