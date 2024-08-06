namespace PatientAdministrationSystem.Application.Entities;

public class PatientHospitalRelation
{
    public Guid PatientId { get; set; }
    public PatientEntity Patient { get; set; } = null!;

    public Guid HospitalId { get; set; }
    public HospitalEntity Hospital { get; set; } = null!;

    public Guid VisitId { get; set; }
    public VisitEntity Visit { get; set; } = null!;
}