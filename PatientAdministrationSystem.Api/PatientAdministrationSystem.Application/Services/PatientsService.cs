using AutoMapper;
using PatientAdministrationSystem.Application.DTOs;
using PatientAdministrationSystem.Application.Entities;
using PatientAdministrationSystem.Application.Interfaces;
using PatientAdministrationSystem.Application.Repositories.Interfaces;

namespace PatientAdministrationSystem.Application.Services
{
    public class PatientsService : IPatientsService
    {
        private readonly IPatientsRepository _patientsRepository;
        private readonly IMapper _mapper;

        public PatientsService(IPatientsRepository patientsRepository, IMapper mapper)
        {
            _patientsRepository = patientsRepository;
            _mapper = mapper;
        }

        #region GetAllPatientsAsync
        /// <summary>
        /// Retrieves all patients from the repository and maps them to PatientDto objects.
        /// </summary>
        /// <returns>A collection of PatientDto objects.</returns>
        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _patientsRepository.GetPatientsAsync();
            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }
        #endregion

        #region GetPatientByIdAsync
        /// <summary>
        /// Retrieves a patient by their unique identifier and maps them to a PatientDto object.
        /// </summary>
        /// <param name="id">The unique identifier of the patient.</param>
        /// <returns>A PatientDto object.</returns>
        public async Task<PatientDto> GetPatientByIdAsync(Guid id)
        {
            var patient = await _patientsRepository.GetPatientByIdAsync(id);
            return _mapper.Map<PatientDto>(patient);
        }
        #endregion

        #region GetPatientVisitsAsync
        /// <summary>
        /// Retrieves all visits for a specific patient and maps them to PatientVisitDto objects.
        /// </summary>
        /// <param name="patientId">The unique identifier of the patient.</param>
        /// <returns>A collection of PatientVisitDto objects.</returns>
        public async Task<IEnumerable<PatientVisitDto>> GetPatientVisitsAsync(Guid patientId)
        {
            var patientHospitals = await _patientsRepository.GetVisitsByPatientIdAsync(patientId);

            var patientVisits = patientHospitals.SelectMany(visit =>
                visit.PatientHospitals.Select(ph => new PatientVisitDto
                {
                    VisitId = visit.Id,
                    Date = visit.Date,
                    HospitalName = ph.Hospital.Name,
                    HospitalAddress = ph.Hospital.Address
                })
            );

            return patientVisits;
        }
        #endregion

        #region CreatePatientAsync
        /// <summary>
        /// Creates a new patient in the repository.
        /// </summary>
        /// <param name="patientDto">The PatientDto object containing patient details.</param>
        public async Task CreatePatientAsync(PatientDto patientDto)
        {
            var patient = _mapper.Map<PatientEntity>(patientDto);
            await _patientsRepository.AddPatientAsync(patient);
        }
        #endregion

        #region UpdatePatientAsync
        /// <summary>
        /// Updates an existing patient in the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the patient.</param>
        /// <param name="patientDto">The PatientDto object containing updated patient details.</param>
        public async Task UpdatePatientAsync(Guid id, PatientDto patientDto)
        {
            var patient = await _patientsRepository.GetPatientByIdAsync(id);
            if (patient == null) throw new KeyNotFoundException("Patient not found");

            _mapper.Map(patientDto, patient);
            await _patientsRepository.UpdatePatientAsync(patient);
        }
        #endregion

        #region DeletePatientAsync
        /// <summary>
        /// Deletes a patient from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the patient.</param>
        public async Task DeletePatientAsync(Guid id)
        {
            var patient = await _patientsRepository.GetPatientByIdAsync(id);
            if (patient == null) throw new KeyNotFoundException("Patient not found");

            await _patientsRepository.DeletePatientAsync(patient);
        }
        #endregion
    }
}
