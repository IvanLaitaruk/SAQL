using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace SAQL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly ILogger _logger;
        private static readonly HttpClient client = new HttpClient();

        public DeviceController(ILogger<DeviceController> logger)
        {
             _logger= logger;
        }

        [HttpGet(Name = "GetJson")]
        public async Task<ActionResult<string>> GetData1()
        {  
            _logger.LogInformation("Attempting to retrieve JSON data");
            string fileUrl = "https://filetransfer.io/data-package/Hf9yRi7x/download";
          
            HttpResponseMessage response = await client.GetAsync(fileUrl);

            if (response.IsSuccessStatusCode)
            {
                // Read the content of the response as a string
                string jsonContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Successfully retrieved JSON data");
                // Return the JSON content as a JsonResult
                return Content(jsonContent, "application/json");
            }
            else
            {
                _logger.LogError("Failed to retrieve JSON file. StatusCode: {StatusCode}", response.StatusCode);
                // Handle the case when the request fails
                return Content("Failed to retrieve JSON file");
            }
        }

        /*[HttpGet(Name = "GetJson")]
        public async Task<ActionResult<string>> GetData2()
        {
            string fileUrl = "https://filetransfer.io/data-package/Hf9yRi7x/download";

            HttpResponseMessage response = await client.GetAsync(fileUrl);

            if (response.IsSuccessStatusCode)
            {
                // Read the content of the response as a string
                string jsonContent = await response.Content.ReadAsStringAsync();

                // Return the JSON content as a JsonResult
                return Content(jsonContent, "application/json");
            }
            else
            {
                // Handle the case when the request fails
                return Content("Failed to retrieve JSON file");
            }
        }*/

    }
}