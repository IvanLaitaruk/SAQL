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

        public PatronController(SAQLContext context)
        {
            _context = context;
        }


        [HttpGet("patients/{patronId}")]
        public async Task<ActionResult<List<PatientDTO>>> GetPatientsByPatronId(int patronId)
        {
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
                return NotFound();
            }
            return patients;
        }

    }
}
