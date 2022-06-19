using System.ComponentModel.DataAnnotations;

namespace LoginSystem.Models.Register
{
    public class LoginSignUpRequest
    {
        [Required]

        public string Username { get; set; }
        [Required]

        public string Email { get; set; }
        [Required]

        public string Password { get; set; }
    }
}
