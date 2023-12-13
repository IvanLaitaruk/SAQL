using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAQL.Contexts;
using SAQL.DTO;

namespace SAQL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatronController : ControllerBase
    {

        private readonly SAQLContext _context;
        private readonly ILogger _logger;
        public PatronController(SAQLContext context, ILogger<PatronController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet("patients/{patronId}")]
        public async Task<ActionResult<List<PatientDTO>>> GetPatientsByPatronId(int patronId)
        {
            _logger.LogInformation("Fetching patients for patron with ID {PatronId}", patronId);
            var patients = await _context.Patients
            .Where(p => p.PatronId == patronId)
            .Select(p => new PatientDTO
            {
                Id = p.Id,
                Name = p.Name,
                Surname = p.Surname,
                DateOfBirth = p.DateOfBirth
            })
            .ToListAsync();

            if (patients == null || !patients.Any())
            {
                _logger.LogWarning("No patients found for patron with ID {PatronId}", patronId);
                return NotFound();
            }
            _logger.LogInformation("Retrieved patients for patron with ID {PatronId}", patronId);
            return patients;
        }

    }
}
