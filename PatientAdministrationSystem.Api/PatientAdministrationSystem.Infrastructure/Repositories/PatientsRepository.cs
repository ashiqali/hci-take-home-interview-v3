using Microsoft.EntityFrameworkCore;
using PatientAdministrationSystem.Application.Entities;
using PatientAdministrationSystem.Application.Repositories.Interfaces;

namespace PatientAdministrationSystem.Infrastructure.Repositories
{
    public class PatientsRepository : IPatientsRepository
    {
        private readonly HciDataContext _context;

        public PatientsRepository(HciDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PatientEntity>> GetPatientsAsync()
        {
            return await _context.Patients.Include(p => p.PatientHospitals)
                                          .ThenInclude(ph => ph.Visit)
                                          .Include(p => p.PatientHospitals)
                                          .ThenInclude(ph => ph.Hospital)
                                          .ToListAsync();
        }

        public async Task<PatientEntity> GetPatientByIdAsync(Guid patientId)
        {
            return await _context.Patients.Include(p => p.PatientHospitals)
                                          .ThenInclude(ph => ph.Visit)
                                          .Include(p => p.PatientHospitals)
                                          .ThenInclude(ph => ph.Hospital)
                                          .FirstOrDefaultAsync(p => p.Id == patientId);
        }

        public async Task<IEnumerable<VisitEntity>> GetVisitsByPatientIdAsync(Guid patientId)
        {
            return await _context.PatientHospitals
                .Where(ph => ph.PatientId == patientId)
                .Include(ph => ph.Visit)
                .ThenInclude(v => v.PatientHospitals)
                .ThenInclude(ph => ph.Hospital)
                .Select(ph => ph.Visit)
                .Distinct() // Ensure unique visits
                .ToListAsync();
        }


        public async Task AddPatientAsync(PatientEntity patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePatientAsync(PatientEntity patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(PatientEntity patient)
        {
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }
    }
}
