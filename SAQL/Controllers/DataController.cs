using Microsoft.AspNetCore.Mvc;
using SAQL.Contexts;
using SAQL.DTO;
using Microsoft.EntityFrameworkCore;
using SAQL.Entities;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using System;

namespace SAQL.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {

        private readonly SAQLContext _context;
        private static readonly HttpClient client = new HttpClient();
       // private readonly ILogger _logger;

        public DataController(SAQLContext context)
        {
            _context = context;
        }
        //public DataController(ILogger<DataController> logger)
        //{
        //    //_context = context;
        //    _logger = logger;
        //}

        [HttpGet("patients/physical/{patientId}")]
        public async Task<ActionResult<List<PhysicalDTO>>> GetPhysicalById(int patientId)
        {
          //  _logger.LogInformation("Fetching physical data for patient with ID {PatientId}", patientId);
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
             //   _logger.LogWarning("No physical data found for patient with ID {PatientId}", patientId);
                return NotFound();
            }
       //     _logger.LogInformation("Retrieved physical data for patient with ID {PatientId}", patientId);
            return physical_data;
        }

        [HttpPost]
        public async Task<ActionResult<PhysiologicalData>> AddPhysical(long patientID)
        {

            //GET FILE
         //   _logger.LogInformation("Adding physical data for patient with ID {PatientId}", patientID);
            DataProcessing dataProcessing = new DataProcessing();
            dataProcessing.setContext(_context);

            string fileUrl = "https://filetransfer.io/data-package/VL9xAjcj/download";

            HttpResponseMessage response = await client.GetAsync(fileUrl);

            string AllRawData = "";
            if (response.IsSuccessStatusCode)
            {
                AllRawData = await response.Content.ReadAsStringAsync();
            }
            
            string[] splits = AllRawData.Split(',');
            Random random = new Random();
            int randomIndex = random.Next(splits.Length);

            string rawData = splits[randomIndex];

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Patient patient = _context.Patients.Find(patientID);

            var physicalEntity = dataProcessing.processData(rawData, patient.DeviceId, patientID, patient.DoctorId);

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = "integration_data.json";
            string JSONFilePath = baseDirectory + relativePath;

            using (StreamWriter writer = new StreamWriter(JSONFilePath, false))
            {
                writer.Write(dataProcessing.GetLastJSON());
            }

            _context.PhysiologicalData.Add(physicalEntity);
            await _context.SaveChangesAsync();
           // _logger.LogInformation("Successfully added physical data for patient with ID {PatientId}", patientID);
            // Return the created resource with a 201 Created status
            return Ok();
        }


    }
}
