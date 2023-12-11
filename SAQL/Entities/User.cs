namespace SAQL.Entities
{
    public enum Role : ushort
    {
        Doctor = 0,
        Patron = 1
    }

    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
