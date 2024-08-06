using PatientAdministrationSystem.Application.DTOs;

namespace PatientAdministrationSystem.Application.Interfaces
{
    public interface IPatientsService
    {
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
        Task<PatientDto> GetPatientByIdAsync(Guid id);
        Task<IEnumerable<PatientVisitDto>> GetPatientVisitsAsync(Guid patientId);
        Task CreatePatientAsync(PatientDto patientDto);
        Task UpdatePatientAsync(Guid id, PatientDto patientDto);
        Task DeletePatientAsync(Guid id);
    }
}
