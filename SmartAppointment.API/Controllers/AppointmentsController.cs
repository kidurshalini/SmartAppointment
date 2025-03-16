using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartAppointment.Application.Interfaces;
using SmartAppointment.Domain.Entities;
using SmartAppointment.Web.Models;
using System.Security.Claims;

namespace SmartAppointment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper; // 🔹 Add AutoMapper

        public AppointmentsController(IAppointmentService appointmentService, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        // ✅ 1️⃣ GET: api/appointments (Admins can view all appointments)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            var appointmentModels = _mapper.Map<List<AppointmentModel>>(appointments);
            return Ok(appointmentModels);
        }

        // ✅ 2️⃣ GET: api/appointments/user (Users can view their own appointments)
        [Authorize(Roles = "User")]
        [HttpGet("user")]
        public async Task<IActionResult> GetUserAppointments()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var appointments = await _appointmentService.GetAppointmentsByUserIdAsync(userId);
            var appointmentModels = _mapper.Map<List<AppointmentModel>>(appointments);
            return Ok(appointmentModels);
        }

        // ✅ 3️⃣ GET: api/appointments/{id} (View Appointment by ID)
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(Guid id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null) return NotFound();

            var appointmentModel = _mapper.Map<AppointmentModel>(appointment);
            return Ok(appointmentModel);
        }

        // ✅ 4️⃣ POST: api/appointments (Users can book an appointment)
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            // 🔹 Convert DTO to Entity
            var appointment = _mapper.Map<Appointment>(model);
            appointment.AssignUser(userId); // Assign logged-in user

            var createdAppointment = await _appointmentService.CreateAppointmentAsync(appointment);
            return CreatedAtAction(nameof(GetAppointmentById), new { id = createdAppointment.Id }, _mapper.Map<AppointmentModel>(createdAppointment));
        }

        // ✅ 5️⃣ PUT: api/appointments/{id} (Users can modify their appointments)
        [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(Guid id, [FromBody] AppointmentModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null || appointment.UserId != userId) return NotFound();

            // 🔹 Map new data onto existing appointment
            _mapper.Map(model, appointment);
            var success = await _appointmentService.UpdateAppointmentAsync(appointment);

            return success ? NoContent() : StatusCode(500, "Error updating appointment");
        }

        // ✅ 6️⃣ DELETE: api/appointments/{id} (Users can cancel their appointments)
        [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null || appointment.UserId != userId) return NotFound();

            var success = await _appointmentService.DeleteAppointmentAsync(id);
            return success ? NoContent() : StatusCode(500, "Error deleting appointment");
        }
    }
}
