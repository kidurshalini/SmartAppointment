using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAppointment.Domain.Entities;
using SmartAppointment.Infrastructure.Data;

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

        //[HttpPost]
        //public async Task<IActionResult> AddSchedule([FromBody] ScheduleModel schedule)
        //{
        //    if (schedule == null)
        //    {
        //        return BadRequest(new { Message = "Invalid data. Schedule details are required." });
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        var errors = ModelState.Values
        //            .SelectMany(v => v.Errors)
        //            .Select(e => e.ErrorMessage)
        //            .ToList();

        //        return BadRequest(new { Message = "Validation failed.", Errors = errors });
        //    }


        //    // Mapping the model to the entity
        //    var scheduleEntity = new scheduleModel
        //    {
        //        Id = schedule.Id,
        //        ProfessionalId = schedule.ProfessionalId, // Include the foreign key
        //        AppointmentDate = schedule.AppointmentDate,
        //        Status = schedule.Status,

        //    };

        //    // Add to database and save changes
        //    _context.scheduler.Add(scheduleEntity);
        //    await _context.SaveChangesAsync();

        //    return Ok(new { Message = "Schedule added successfully!" });
        //}


        [HttpPost]
        public async Task<IActionResult> AddSchedule([FromBody] Web.Models.ScheduleModel schedule)
        {
            // Validate the input
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

            try
            {
                // Map the ScheduleModel to the scheduleModel entity
                var scheduleEntity = new Domain.Entities.scheduleModel
                {
                    Id = Guid.NewGuid(), // Generate a new GUID for the schedule
                    ProfessionalId = schedule.ProfessionalId,
                    AppointmentDate = schedule.AppointmentDate,
                    Status = schedule.Status,
                    AppointmentTime = schedule.AppointmentTime,
                    CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Sri Lanka Standard Time")),
                    UpdatedAt = null // Set UpdatedAt to null initially
                };

                // Add the entity to the database
                _context.scheduler.Add(scheduleEntity);
                await _context.SaveChangesAsync();

                // Return a success response
                return Ok(new { Message = "Schedule added successfully!", ScheduleId = scheduleEntity.Id });
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog or NLog)
                Console.WriteLine($"Exception: {ex}");

                // Return a 500 Internal Server Error response
                return StatusCode(500, new { Message = "An error occurred while adding the schedule.", Error = ex.Message });
            }
        }


        [HttpGet]
            public async Task<ActionResult<List<Web.Models.ScheduleModel>>> GetSchedules(string appointmentDateFilter = "")
            {
                var query = _context.scheduler.AsQueryable();

                if (!string.IsNullOrEmpty(appointmentDateFilter))
                {
                    var filterDate = DateTime.Parse(appointmentDateFilter);
                    query = query.Where(s => s.AppointmentDate.Date == filterDate.Date);
                }

                var schedules = await query
                    .Select(s => new ScheduleModel
                    {
                        Id = s.Id,
                        ProfessionalId = s.ProfessionalId,
                        AppointmentDate = s.AppointmentDate,
                        AppointmentTime =s.AppointmentTime,
                        Status = s.Status
                    })
                    .ToListAsync();

                return Ok(schedules);
            }


        [HttpGet("{id}")]
        public async Task<ActionResult<Web.Models.ScheduleModel>> GetScheduleById(Guid id)
        {
            var Schedulemodel = await _context.scheduler.FindAsync(id);
            if (Schedulemodel == null)
            {
                return NotFound();
            }

            // Return the professional data
            return Ok(Schedulemodel);

        }



        // ✅ Update a professional
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(Guid id, [FromBody] Web.Models.ScheduleModel scheduleModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Schedulemodel = await _context.scheduler.FindAsync(id);

            if (Schedulemodel == null)
            {
                return NotFound(new { Message = "Professional not found." });
            }

            Schedulemodel.ProfessionalId = scheduleModel.ProfessionalId;
            Schedulemodel.AppointmentDate = scheduleModel.AppointmentDate;
            Schedulemodel.Status = scheduleModel.Status;
            Schedulemodel.AppointmentTime = scheduleModel.AppointmentTime;

            Schedulemodel.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Sri Lanka Standard Time"));


            await _context.SaveChangesAsync();

            return Ok(new { Message = "Professional updated successfully!" });
        }

        // DELETE: api/schedule/{id}
        [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteSchedule(Guid id)
            {
                var schedule = await _context.scheduler.FindAsync(id);
                if (schedule == null)
                {
                    return NotFound();
                }

                _context.scheduler.Remove(schedule);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }
