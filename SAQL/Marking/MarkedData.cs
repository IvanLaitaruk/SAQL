using SAQL.Entities;

namespace SAQL.Marking
{
    public class MarkedData
    {
        public DateTime Date { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public Device Device { get; set; }
        public PhysiologicalData PhysiologicalData { get; set; }
    }
}
