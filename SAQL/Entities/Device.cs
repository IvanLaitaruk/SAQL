namespace SAQL.Entities
{
    public enum OperationSystem
    {
        WatchOS = 0, //IOS
        WearOS = 1, //Android
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
