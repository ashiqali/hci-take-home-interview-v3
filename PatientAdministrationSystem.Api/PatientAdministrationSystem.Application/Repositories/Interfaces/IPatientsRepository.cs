using PatientAdministrationSystem.Application.Entities;

namespace PatientAdministrationSystem.Application.Repositories.Interfaces;

public interface IPatientsRepository
{
    Task<IEnumerable<PatientEntity>> GetPatientsAsync();
    Task<PatientEntity?> GetPatientByIdAsync(Guid patientId);
    Task<IEnumerable<VisitEntity>> GetVisitsByPatientIdAsync(Guid patientId);
    Task AddPatientAsync(PatientEntity patient);
    Task UpdatePatientAsync(PatientEntity patient);
    Task DeletePatientAsync(PatientEntity patient);
}