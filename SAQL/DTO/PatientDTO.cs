using SAQL.Entities;

namespace SAQL.DTO
{
    public class PatientDTO
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
      
    }
}
