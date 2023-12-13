using SAQL.Entities;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SAQL.Marking
{
    public class JSONMarkedData : MarkedData
    {
        public string getJSON()
        {
            var json = new
            {
                Date = PhysiologicalData?.LastUpdate.ToString() ?? "",
                PatientName = Patient?.Name ?? "",
                PatientSurname = Patient?.Surname ?? "",
                DoctorName = Doctor?.Name ?? "",
                DoctorSurname = Doctor?.Surname ?? "",
                DeviceModel = Device?.Model ?? "",
                DeviceBrand = Device?.Brand ?? "",
                Pressure = new
                {
                    Systolic = PhysiologicalData?.SystolicPressure ?? 0,
                    Diastolic = PhysiologicalData?.DiastolicPressure ?? 0
                },
                Pulse = PhysiologicalData?.Pulse ?? 0,
                StepsAmount = PhysiologicalData?.StepsAmount ?? 0,
                Temperature = PhysiologicalData?.Temperature ?? 0,
                Oxygen = PhysiologicalData?.Oxygen ?? 0
            };
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            return JsonSerializer.Serialize(json, options);
        }
    }
}
