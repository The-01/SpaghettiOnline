using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpaghettiOnline.Models
{
    public class User
    {
        [Display(Name = "Username")]
        [Required, MinLength(3, ErrorMessage = "Username should be at least 3 characters!")]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password), MinLength(4, ErrorMessage = "Password should be at least 4 characters!")]
        public string Password { get; set; }
    }
}
