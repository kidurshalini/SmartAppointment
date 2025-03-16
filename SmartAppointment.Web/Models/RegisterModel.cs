using System.ComponentModel.DataAnnotations;

namespace SmartAppointment.Web.Models
{
    public class RegisterModel
    {
        //public string Email { get; set; }
        //public string Password { get; set; }
        //public string Role { get; set; } // "Admin", "Professional", or "User"

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; }

    }
}
