using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAppointment.Domain.Entities;
using SmartAppointment.Infrastructure.Data;
using SmartAppointment.Web.Models;
using System.Threading.Tasks;

[ApiController]
[Route("api/professional")]
public class ProfessionalController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProfessionalController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Endpoint to add a new professional
    [HttpPost]
    public async Task<IActionResult> AddProfessional([FromBody] ProfessionalModel professional)
    {
        if (professional == null)
        {
            return BadRequest(new { Message = "Invalid data. Professional details are required." });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingProfessional = await _context.Professionals
     .FirstOrDefaultAsync(p => p.SLMC == professional.SLMC);

        if (existingProfessional != null)
        {
            return BadRequest(new { Message = "The SLMC number is already taken." });
        }


        // Mapping the model to the entity
        var professionalEntity = new Professional(
            name: professional.Name,
            specialization: professional.Specialization,
            email: professional.Email,
            phoneNumber: professional.PhoneNumber,
            slmc: professional.SLMC
        );

        // Add to database and save changes
        _context.Professionals.Add(professionalEntity);
        await _context.SaveChangesAsync();

        // Return success response
        return Ok(new { Message = "Professional added successfully!" });
    }

    // ✅ Get all professionals (supports filtering)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Professional>>> GetProfessionals([FromQuery] string specialization = null)
    {
        var query = _context.Professionals.AsQueryable();

        if (!string.IsNullOrEmpty(specialization))
        {
            query = query.Where(p => EF.Functions.Like(p.Specialization, $"%{specialization}%"));
        }

        var professionals = await query.ToListAsync();

        if (!professionals.Any())
        {
            return NotFound(new { Message = "No professionals found." });
        }

        return Ok(professionals);
    }

    // ✅ Get a single professional by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Professional>> GetProfessionalById(int id)
    {
        var professional = await _context.Professionals.FindAsync(id);

        if (professional == null)
        {
            return NotFound(new { Message = "Professional not found." });
        }

        return Ok(professional);
    }

    // ✅ Update a professional
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfessional(int id, [FromBody] ProfessionalModel updatedProfessional)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var professional = await _context.Professionals.FindAsync(id);

        if (professional == null)
        {
            return NotFound(new { Message = "Professional not found." });
        }

        professional.Name = updatedProfessional.Name;
        professional.Specialization = updatedProfessional.Specialization;
        professional.Email = updatedProfessional.Email;
        professional.PhoneNumber = updatedProfessional.PhoneNumber;
        professional.SLMC = updatedProfessional.SLMC;

        await _context.SaveChangesAsync();

        return Ok(new { Message = "Professional updated successfully!" });
    }

    // ✅ Delete a professional
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProfessional(Guid id)
    {
        var professional = await _context.Professionals.FindAsync(id);

        if (professional == null)
        {
            return NotFound(new { Message = "Professional not found." });
        }

        _context.Professionals.Remove(professional);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Professional deleted successfully!" });
    }

}