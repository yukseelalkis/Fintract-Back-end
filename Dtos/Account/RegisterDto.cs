using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Account
{
    public class RegisterDto
    {
        // username
        [Required]
        public string? UserName { get; set; }
        //Email
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        /// password
        [Required]
        public string? Password { get; set; }
    }
}