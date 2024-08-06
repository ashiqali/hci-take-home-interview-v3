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

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _patientsRepository.GetPatientsAsync();
            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task<PatientDto> GetPatientByIdAsync(Guid id)
        {
            var patient = await _patientsRepository.GetPatientByIdAsync(id);
            return _mapper.Map<PatientDto>(patient);
        }

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

        public async Task CreatePatientAsync(PatientDto patientDto)
        {
            var patient = _mapper.Map<PatientEntity>(patientDto);
            await _patientsRepository.AddPatientAsync(patient);
        }

        public async Task UpdatePatientAsync(Guid id, PatientDto patientDto)
        {
            var patient = await _patientsRepository.GetPatientByIdAsync(id);
            if (patient == null) throw new KeyNotFoundException("Patient not found");

            _mapper.Map(patientDto, patient);
            await _patientsRepository.UpdatePatientAsync(patient);
        }

        public async Task DeletePatientAsync(Guid id)
        {
            var patient = await _patientsRepository.GetPatientByIdAsync(id);
            if (patient == null) throw new KeyNotFoundException("Patient not found");

            await _patientsRepository.DeletePatientAsync(patient);
        }
    }
}
