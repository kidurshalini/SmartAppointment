using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Domain.Entities
{
  public class AppointmentModel
    {
       
  
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string PatientName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public Guid SchedulerId { get; set; }

        [ForeignKey("SchedulerId")]
        public scheduleModel Scheduler { get; set; }
    }
}

