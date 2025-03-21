using System;

namespace SmartAppointment.Domain.Entities
{
    public class scheduleModel
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Auto-generate a new GUID if not provided
        public Guid ProfessionalId { get; set; } // Foreign key to the Professional table
        public DateTime AppointmentDate { get; set; } // Date of the appointment
        public string Status { get; set; } // Status of the appointment (e.g., Scheduled, Cancelled, Completed)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Auto-set to current UTC time
        public DateTime? UpdatedAt { get; set; } // Nullable field for updates
        public string AppointmentTime { get; set; } // Time of the appointment (e.g., "10:00")

        // Navigation property for the Professional entity
        public Professional Professional { get; set; }
        //public ICollection<AppointmentModel> Appointments { get; set; }

        public ICollection<AppointmentModel> Appointments { get; set; }

        // Default constructor (required for EF Core)
        public scheduleModel() { }
    }
}