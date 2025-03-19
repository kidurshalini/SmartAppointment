using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAppointment.Domain.Entities;
using SmartAppointment.Infrastructure.Data;
using SmartAppointment.Infrastructure.Migrations;
using SmartAppointment.Web.Models;
using SmartAppointment.Web.Pages;
using System.Threading.Tasks;

namespace SmartAppointment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddSchedule([FromBody] ScheduleModel schedule)
        {
            if (schedule == null)
            {
                return BadRequest(new { Message = "Invalid data. Schedule details are required." });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { Message = "Validation failed.", Errors = errors });
            }

       
            // Mapping the model to the entity
            var scheduleEntity = new scheduleModel
            {
               
                ProfessionalId = schedule.ProfessionalId, // Include the foreign key
                AppointmentDate = schedule.AppointmentDate,
                Status = schedule.Status
            };

            // Add to database and save changes
            _context.scheduler.Add(scheduleEntity);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Schedule added successfully!" });
        }
    }
  }