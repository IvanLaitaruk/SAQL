using Microsoft.AspNetCore.Mvc;
using SAQL.Contexts;
using SAQL.Entities;
using Microsoft.EntityFrameworkCore;
using SAQL.DTO;

namespace SAQL.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly SAQLContext _context;

        public PatientController(SAQLContext context)
        {
            _context = context;
        }

      
        [HttpGet("GetByDoctorId/{doctorId}")]
        public async Task<ActionResult<List<PatientDTO>>> GetPatientsByDoctorId(int doctorId)
        {
            var patients = await _context.Patients
            .Where(p => p.DoctorId == doctorId)
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
