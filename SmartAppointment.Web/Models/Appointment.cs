using SmartAppointment.Domain.Entities;
using SmartAppointment.Web.Models;
using System.ComponentModel.DataAnnotations;

public class Appointment
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Patient name is required.")]
    [StringLength(100, ErrorMessage = "Patient name cannot exceed 100 characters.")]
    public string PatientName { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number.")]
    [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Appointment date is required.")]
    public DateTime AppointmentDate { get; set; }

    [Required(ErrorMessage = "Created date is required.")]
    public DateTime CreatedDate { get; set; }

    [Required(ErrorMessage = "Scheduler ID is required.")]
    public Guid SchedulerId { get; set; }

    // Navigation property
    public ScheduleModel Scheduler { get; set; }
}
