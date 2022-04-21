using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class LoginDto
    {
        [Required]
        public string userName { get; set; }

        [Required]
        public string password { get; set; }
    }
}