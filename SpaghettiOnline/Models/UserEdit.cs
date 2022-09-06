using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpaghettiOnline.Models
{
    public class UserEdit
    {

        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), MinLength(4, ErrorMessage = "Password should be at least 4 characters!")]
        public string Password { get; set; }

        public UserEdit()
        {

        }

        public UserEdit(AppUser appUser)
        {
            Email = appUser.Email;
            Password = appUser.PasswordHash;
        }
    }
}
