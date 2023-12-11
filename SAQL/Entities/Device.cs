namespace SAQL.Entities
{
    public enum OperationSystem
    {
        IOS = 0,
        Android = 1,
    }
    public class Device
    {
        public long Id { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public OperationSystem OS { get; set; }
        public long PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
