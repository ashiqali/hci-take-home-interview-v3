namespace PatientAdministrationSystem.Application.Entities;

public class PatientHospitalRelation
{
    public Guid PatientId { get; set; }
    public PatientEntity Patient { get; set; }

    public Guid HospitalId { get; set; }
    public HospitalEntity Hospital { get; set; }

    public Guid VisitId { get; set; }
    public VisitEntity Visit { get; set; }
}