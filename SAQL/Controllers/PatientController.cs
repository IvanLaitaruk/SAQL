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

        [HttpGet("patients/{doctorId}")]
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

        [HttpGet]
        public async Task<ActionResult<List<PatientDTO>>> GetDoctorPatientsByQuery(int doctorId,string searchQuery)
        {

            int.TryParse(searchQuery, out var userId);
            var result = await _context.Patients.Where(p =>
                p.DoctorId == doctorId && (
                    p.Name.Contains(searchQuery) ||
                    p.Surname.Contains(searchQuery) ||
                    p.Id == userId)).Select(p => new PatientDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Surname = p.Surname,
                        DateOfBirth = p.DateOfBirth
                    })
            .ToListAsync();
            
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            return result;
        }


        [HttpGet("{patientId}")]
        public async Task<ActionResult<PatientDTO>> GetPatientById(int patientId)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            

            if (patient == null)
            {
                return NotFound();
            }

            var result = new PatientDTO
            {
                Id = patient.Id,
                Name = patient.Name,
                Surname = patient.Surname,
                DateOfBirth = patient.DateOfBirth
            };

            return result;
        }

    }
}
