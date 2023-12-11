namespace SAQL.Entities
{
    public class Patient
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public long DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public long PatronId { get; set; }
        public Patron Patron { get; set; }
        public long DeviceId { get; set; }
        public Device Device { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan IntervalTime { get; set; }
    }
}
