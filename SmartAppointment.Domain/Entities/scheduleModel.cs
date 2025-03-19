using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartAppointment.Domain.Entities
{
   public class scheduleModel
    {

        public Guid Id { get; set; }
        public Guid ProfessionalId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }  
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Professional Professional { get; set; }

        //public scheduleModel(Guid Id, Guid ProfessionalId, DateTime AppointmentDate, string Status, DateTime CreatedAt)
        //    {
        //        Id = Id;
        //        ProfessionalId = ProfessionalId;
        //        AppointmentDate = AppointmentDate;
        //        Status = Status;
        //        CreatedAt = CreatedAt;

        //    }

        public scheduleModel( DateTime appointmentDate, string status, Guid professionalID)
        {
           AppointmentDate = appointmentDate;
            Status = status;

            ProfessionalId = professionalID;
        }
        public scheduleModel() { }
    }

 

}
