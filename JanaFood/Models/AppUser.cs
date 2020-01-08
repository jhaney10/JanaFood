﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JanaFood.Models
{
    public class AppUser : IdentityUser
    {
        public string City { get; set; }
    }
}
