﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaghettiOnline.Models
{
    public class AppUser : IdentityUser
    {
        public string occupation { get; set; }
    }
}
