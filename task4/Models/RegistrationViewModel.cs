using System.ComponentModel.DataAnnotations;

namespace task4.Models
{
    public class RegistrationViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
