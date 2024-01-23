using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Angular_Authentication_with_JWT.Dto_s;
    
    public class UserLogin
    { 
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

