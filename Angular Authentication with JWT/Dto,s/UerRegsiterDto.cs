using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace Angular_Authentication_with_JWT.Dto_s
{
    public class UerRegsiterDto
    {
        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
