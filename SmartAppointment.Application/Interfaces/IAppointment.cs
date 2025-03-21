using System;
using System.Threading.Tasks;
using SmartAppointment.Domain.Entities;


namespace SmartAppointment.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<AppointmentModel> CreateAppointmentAsync(AppointmentModel appointment);
        Task<AppointmentModel> GetAppointmentByIdAsync(Guid id);
    }
}