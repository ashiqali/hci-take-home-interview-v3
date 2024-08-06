namespace PatientAdministrationSystem.Application.Entities;

public class HospitalEntity : Entity<Guid>
{
    public string Name { get; set; }
    public string Address { get; set; }
    public ICollection<PatientHospitalRelation> PatientHospitals { get; set; }
}