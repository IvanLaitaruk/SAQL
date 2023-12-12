namespace SAQL.Entities
{
    public enum OperationSystem
    {
        None = 0,
        WatchOS = 1, //IOS
        WearOS = 2, //Android
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
