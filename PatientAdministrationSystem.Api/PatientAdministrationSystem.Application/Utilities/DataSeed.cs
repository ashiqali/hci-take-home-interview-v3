using PatientAdministrationSystem.Application.Entities;
namespace PatientAdministrationSystem.Application.Utilities
{
    public class DataSeed
    {
        public List<HospitalEntity> Hospitals { get; set; } = new List<HospitalEntity>();
        public List<PatientEntity> Patients { get; set; } = new List<PatientEntity>();
        public List<VisitEntity> Visits { get; set; } = new List<VisitEntity>();
    }
}
