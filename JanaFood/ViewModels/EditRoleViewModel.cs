using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JanaFood.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        [Required]
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
