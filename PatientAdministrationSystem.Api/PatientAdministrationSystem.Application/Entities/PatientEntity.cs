namespace PatientAdministrationSystem.Application.Entities;

public class PatientEntity : Entity<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public ICollection<PatientHospitalRelation> PatientHospitals { get; set; } = null!;
}