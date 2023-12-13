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
        private readonly ILogger _logger;
        public PatientController(SAQLContext context, ILogger<PatientController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("patients/{doctorId}")]
        public async Task<ActionResult<List<PatientDTO>>> GetPatientsByDoctorId(int doctorId)
        {
            _logger.LogInformation("Fetching patients for doctor with ID {DoctorId}", doctorId);
            var patients = await _context.Patients
            .Where(p => p.DoctorId == doctorId)
            .Select(p => new PatientDTO
            {
                Id = p.Id,
                Name = p.Name,
                Surname = p.Surname,
                DateOfBirth = p.DateOfBirth,
                Diagnose = p.Diagnose,
                DeviceModel = GetDeviceName(p.DeviceId)
            })
            .ToListAsync();

            if (patients == null || !patients.Any())
            {
                _logger.LogWarning("No patients found for doctor with ID {DoctorId}", doctorId);
                return NotFound(); 
            }
            _logger.LogInformation("Retrieved patients for doctor with ID {DoctorId}", doctorId);
            return patients;
        }

        [HttpGet]
        public async Task<ActionResult<List<PatientDTO>>> GetDoctorPatientsByQuery(int doctorId,string? searchQuery = "")
        {
            _logger.LogInformation("Fetching patients for doctor with ID {DoctorId} and search query {SearchQuery}", doctorId, searchQuery);
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
                        DateOfBirth = p.DateOfBirth,
                        Diagnose = p.Diagnose,
                        DeviceModel = GetDeviceName(p.DeviceId)
                    })
            .ToListAsync();
            
            if (result == null || !result.Any())
            {
                _logger.LogWarning("No patients found for doctor with ID {DoctorId} and search query {SearchQuery}", doctorId, searchQuery);
                return NotFound();
            }
            _logger.LogInformation("Retrieved patients for doctor with ID {DoctorId} and search query {SearchQuery}", doctorId, searchQuery);
            return result;
        }


        [HttpGet("{patientId}")]
        public async Task<ActionResult<PatientDTO>> GetPatientById(int patientId)
        {
            _logger.LogInformation("Fetching patient with ID {PatientId}", patientId);
            var patient = await _context.Patients.FindAsync(patientId);
            

            if (patient == null)
            {
                _logger.LogWarning("No patient found with ID {PatientId}", patientId);
                return NotFound();
            }

            var result = new PatientDTO
            {
                Id = patient.Id,
                Name = patient.Name,
                Surname = patient.Surname,
                DateOfBirth = patient.DateOfBirth,
                Diagnose = patient.Diagnose,
                DeviceModel = GetDeviceName(patient.DeviceId)
            };
            _logger.LogInformation("Retrieved patient with ID {PatientId}", patientId);
            return result;
        }
        private static string GetDeviceName(long deviceId)
        {
            using var dbContext = new SAQLContext();
            var device = dbContext.Devices.FirstOrDefault(d => d.Id == deviceId);
            return device?.Model ?? "Unknown"; 
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTimerInfo(long patientId, int interval, DateTime startTime)
        {

            var existingPatient = await _context.Patients.FindAsync(patientId);

            if (existingPatient == null)
            {
                return NotFound("Patient not found");
            }

            TimeSpan IntervalTime = TimeSpan.FromHours(interval);


            existingPatient.IntervalTime = IntervalTime;
            existingPatient.DateTime = startTime;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(_context.Patients.Any(e => e.Id == patientId)))
                {
                    return NotFound("Patient not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


    }
}
