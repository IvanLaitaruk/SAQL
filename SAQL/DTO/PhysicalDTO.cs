using SAQL.Entities;

namespace SAQL.DTO
{
    public class PhysicalDTO
    {
        public long Id { get; set; }
        public int SystolicPressure { get; set; }
        public int DiastolicPressure { get; set; }
        public int Pulse { get; set; }
        public int StepsAmount { get; set; }
        public double Temperature { get; set; }
        public int Oxygen { get; set; }
        public DateTime LastUpdate { get; set; }
 
    }
}
