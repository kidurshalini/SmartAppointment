using System.ComponentModel.DataAnnotations;

namespace SmartAppointment.Web.Models
{
    public class ProfessionalModel
    {

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Specialization is required.")]
        public string Specialization { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }



        [Required(ErrorMessage = "SLMC is required.")]
        [StringLength(20)]  // Adjust length if needed
        public string SLMC { get; set; }
        public Guid Id { get;  set; }

        //[Required(ErrorMessage = "UserId is required.")]
        //public string UserId { get; set; 
        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Sri Lanka Standard Time"));

    }
}
