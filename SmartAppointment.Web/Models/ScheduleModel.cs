//using System;
//using System.ComponentModel.DataAnnotations;

//namespace SmartAppointment.Web.Models
//{
//    public class ScheduleModel
//    {
//        public Guid Id { get; set; }

//        //[Required(ErrorMessage = "ProfessionalId is required.")]
//        //public Guid ProfessionalId { get; set; }

//        [Required(ErrorMessage = "AppointmentDate is required.")]
//        [FutureDate(ErrorMessage = "AppointmentDate must be a future date.")]
//        public DateTime AppointmentDate { get; set; } = DateTime.Today;

//        [Required(ErrorMessage = "Status is required.")]
//        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
//        public string Status { get; set; }

//      //  public DateTime CreatedAt { get; set; }

//        //public DateTime? UpdatedAt { get; set; }
//    }

//    // Custom validation attribute to ensure the date is in the future
//    public class FutureDateAttribute : ValidationAttribute
//    {
//        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//        {
//            if (value is DateTime date)
//            {
//                if (date >= DateTime.Now)
//                {
//                    return ValidationResult.Success;
//                }
//                else
//                {
//                    return new ValidationResult(ErrorMessage ?? "The date must be in the future.");
//                }
//            }
//            return new ValidationResult("Invalid date.");
//        }
//    }
//}

using System;
using System.ComponentModel.DataAnnotations;

namespace SmartAppointment.Web.Models
{
    public class ScheduleModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "ProfessionalId is required.")]
        public Guid ProfessionalId { get; set; } // Foreign key


        [Required(ErrorMessage = "AppointmentDate is required.")]
        [FutureDate(ErrorMessage = "AppointmentDate must be today or a future date.")]
        public DateTime AppointmentDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string Status { get; set; }
    }

    // Custom validation attribute to ensure the date is today or in the future
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date >= DateTime.Today) // Allow today's date
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(ErrorMessage ?? "The date must be today or a future date.");
                }
            }
            return new ValidationResult("Invalid date.");
        }
    }
}