namespace PatientAdministrationSystem.Application.Entities;

public class PatientEntity : Entity<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public ICollection<PatientHospitalRelation> PatientHospitals { get; set; }
}