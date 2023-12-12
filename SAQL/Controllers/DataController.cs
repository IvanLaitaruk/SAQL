using Microsoft.AspNetCore.Mvc;
using SAQL.Contexts;
using SAQL.DTO;
using Microsoft.EntityFrameworkCore;

namespace SAQL.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {

        private readonly SAQLContext _context;

        public DataController(SAQLContext context)
        {
            _context = context;
        }

        [HttpGet("patients/physical/{patientId}")]
        public async Task<ActionResult<List<PhysicalDTO>>> GetPhysicalById(int patientId)
        {
            var physical_data = await _context.PhysiologicalData
            .Where(p => p.PatientId == patientId)
            .Select(p => new PhysicalDTO
            {
                Id = p.Id,
                DiastolicPressure = p.DiastolicPressure,
                SystolicPressure = p.SystolicPressure,
                Pulse = p.Pulse,
                StepsAmount = p.StepsAmount,
                LastUpdate = p.LastUpdate,
                Temperature = p.Temperature,
                Oxygen  = p.Oxygen
            })
            .ToListAsync();

            if (physical_data == null || !physical_data.Any())
            {
                return NotFound();
            }
            return physical_data;
        }




    }
}
