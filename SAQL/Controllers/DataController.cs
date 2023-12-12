using Microsoft.AspNetCore.Mvc;
using SAQL.Contexts;
using SAQL.DTO;
using Microsoft.EntityFrameworkCore;
using SAQL.Entities;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SAQL.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {

        private readonly SAQLContext _context;
        private static readonly HttpClient client = new HttpClient();

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

        [HttpPost]
        public async Task<ActionResult<PhysiologicalData>> AddPhysical(long patientID)
        {

            //GET FILE

            DataProcessing dataProcessing = new DataProcessing();
            dataProcessing.setContext(_context);

            string fileUrl = "https://filetransfer.io/data-package/Hf9yRi7x/download";

            HttpResponseMessage response = await client.GetAsync(fileUrl);

            string rawData = "";
            if (response.IsSuccessStatusCode)
            {
                // Read the content of the response as a string
               rawData = await response.Content.ReadAsStringAsync();
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // ШОСЬ РОБИМО
            Patient patient = _context.Patients.Find(patientID);

            var physicalEntity = dataProcessing.processData(rawData, patient.DeviceId, patientID, patient.DoctorId);

            //ПЕРЕДАТИ ЦЮ ЗАЛУПУ
            _context.PhysiologicalData.Add(physicalEntity);
            await _context.SaveChangesAsync();

            // Return the created resource with a 201 Created status
            return Ok();
        }


    }
}
