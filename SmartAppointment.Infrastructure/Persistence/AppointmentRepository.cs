using Microsoft.EntityFrameworkCore;
using SmartAppointment.Application.Interfaces;
using SmartAppointment.Domain.Entities;
using SmartAppointment.Infrastructure.Data;

namespace SmartAppointment.Infrastructure.Persistence
{
    public class AppointmentRepository : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Implement GetAppointmentsAsync() with CancellationToken
        public async Task<List<Appointment>> GetAppointmentsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Appointments.ToListAsync(cancellationToken);
        }

        // Implement GetAllAppointmentsAsync() with CancellationToken
        public async Task<List<Appointment>> GetAllAppointmentsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Appointments.ToListAsync(cancellationToken);  // Retrieves all appointments
        }

        // Implement GetAppointmentsByUserIdAsync() with CancellationToken
        public async Task<List<Appointment>> GetAppointmentsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.Appointments
                .Where(a => a.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        // Implement GetAppointmentsByProfessionalIdAsync() with CancellationToken
        public async Task<List<Appointment>> GetAppointmentsByProfessionalIdAsync(Guid professionalId, CancellationToken cancellationToken = default)
        {
            return await _context.Appointments
                .Where(a => a.ProfessionalId == professionalId)
                .ToListAsync(cancellationToken);
        }

        // Implement GetAppointmentByIdAsync() with CancellationToken
        public async Task<Appointment> GetAppointmentByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Appointments.FindAsync(new object[] { id }, cancellationToken);
        }

        // Implement CreateAppointmentAsync() with CancellationToken
        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync(cancellationToken);
            return appointment;
        }

        // Implement UpdateAppointmentAsync() with CancellationToken
        public async Task<bool> UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default)
        {
            _context.Appointments.Update(appointment);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        // Implement DeleteAppointmentAsync() with CancellationToken
        public async Task<bool> DeleteAppointmentAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var appointment = await _context.Appointments.FindAsync(new object[] { id }, cancellationToken);
            if (appointment == null) return false;

            _context.Appointments.Remove(appointment);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
