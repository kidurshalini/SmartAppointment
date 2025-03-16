using SmartAppointment.Domain.Entities;

namespace SmartAppointment.Application.Interfaces
{
    public interface IAppointmentService
    {
        // 🔹 Admins can get all appointments
        Task<List<Appointment>> GetAllAppointmentsAsync(CancellationToken cancellationToken = default);

        // 🔹 Users can get their own appointments
        Task<List<Appointment>> GetAppointmentsByUserIdAsync(string userId, CancellationToken cancellationToken = default);

        // 🔹 Professionals can get their own appointments (FIX: Changed string → Guid)
        Task<List<Appointment>> GetAppointmentsByProfessionalIdAsync(Guid professionalId, CancellationToken cancellationToken = default);

        // 🔹 Get appointment by ID
        Task<Appointment> GetAppointmentByIdAsync(Guid id, CancellationToken cancellationToken = default);

        // 🔹 Create an appointment
        Task<Appointment> CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default);

        // 🔹 Update an appointment
        Task<bool> UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default);

        // 🔹 Delete an appointment
        Task<bool> DeleteAppointmentAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
