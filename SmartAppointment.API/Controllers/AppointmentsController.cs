using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAppointment.Domain.Entities;
using SmartAppointment.Infrastructure.Data;
using SmartAppointment.Web.Models;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AppointmentsController(ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpPost]
    public async Task<IActionResult> CreateAppointment([FromBody] AppointmentModel appointment)
    {
        // Check if the appointment data is null
        if (appointment == null)
        {
            return BadRequest(new { Message = "Invalid data. Appointment details are required." });
        }

        // Validate the model
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Map the model to the database entity (if needed)
        var appointmentEntity = new AppointmentModel
        {
            PatientName = appointment.PatientName,
            PhoneNumber = appointment.PhoneNumber,
            CreatedDate = DateTime.UtcNow,
            SchedulerId = appointment.SchedulerId,
            // Map other properties as needed
        };

        // Add the appointment entity to the database
        _context.Appointments.Add(appointmentEntity);
        await _context.SaveChangesAsync();

        // Return success response
        return Ok(new { Message = "Appointment created successfully!" });
    }

}
