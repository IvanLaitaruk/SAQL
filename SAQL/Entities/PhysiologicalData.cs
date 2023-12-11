namespace SAQL.Entities
{
    public class PhysiologicalData
    {
        public long Id { get; set; }
        public int SystolicPressure { get; set; }
        public int DiastolicPressure { get; set; }
        public int Pulse { get; set; }
        public int StepsAmount { get; set; }
        public double Temperature { get; set; }
        public int Oxygen { get; set; }
        public DateTime LastUpdate { get; set; }
        public long PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
