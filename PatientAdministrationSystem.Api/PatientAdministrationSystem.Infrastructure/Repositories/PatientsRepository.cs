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

        #region GetPatientsAsync
        /// <summary>
        /// Retrieves all patients from the database, including their associated hospitals and visits.
        /// </summary>
        /// <returns>A collection of PatientEntity objects.</returns>
        public async Task<IEnumerable<PatientEntity>> GetPatientsAsync()
        {
            return await _context.Patients.Include(p => p.PatientHospitals)
                                          .ThenInclude(ph => ph.Visit)
                                          .Include(p => p.PatientHospitals)
                                          .ThenInclude(ph => ph.Hospital)
                                          .ToListAsync();
        }
        #endregion

        #region GetPatientByIdAsync
        /// <summary>
        /// Retrieves a patient by their unique identifier, including their associated hospitals and visits.
        /// </summary>
        /// <param name="patientId">The unique identifier of the patient.</param>
        /// <returns>A PatientEntity object or null if not found.</returns>
        public async Task<PatientEntity?> GetPatientByIdAsync(Guid patientId)
        {
            var patient = await _context.Patients.Include(p => p.PatientHospitals)
                                                 .ThenInclude(ph => ph.Visit)
                                                 .Include(p => p.PatientHospitals)
                                                 .ThenInclude(ph => ph.Hospital)
                                                 .FirstOrDefaultAsync(p => p.Id == patientId);

            return patient ?? null;
        }
        #endregion

        #region GetVisitsByPatientIdAsync
        /// <summary>
        /// Retrieves all visits for a specific patient, including associated hospitals.
        /// </summary>
        /// <param name="patientId">The unique identifier of the patient.</param>
        /// <returns>A collection of VisitEntity objects.</returns>
        public async Task<IEnumerable<VisitEntity>> GetVisitsByPatientIdAsync(Guid patientId)
        {
            return await _context.PatientHospitals
                .Where(ph => ph.PatientId == patientId)
                .Include(ph => ph.Visit)
                .ThenInclude(v => v.PatientHospitals)
                .ThenInclude(ph => ph.Hospital)
                .Select(ph => ph.Visit)
                .Distinct()
                .ToListAsync();
        }
        #endregion

        #region AddPatientAsync
        /// <summary>
        /// Adds a new patient to the database.
        /// </summary>
        /// <param name="patient">The PatientEntity object containing patient details.</param>
        public async Task AddPatientAsync(PatientEntity patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region UpdatePatientAsync
        /// <summary>
        /// Updates an existing patient in the database.
        /// </summary>
        /// <param name="patient">The PatientEntity object containing updated patient details.</param>
        public async Task UpdatePatientAsync(PatientEntity patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region DeletePatientAsync
        /// <summary>
        /// Deletes a patient from the database.
        /// </summary>
        /// <param name="patient">The PatientEntity object representing the patient to be deleted.</param>
        public async Task DeletePatientAsync(PatientEntity patient)
        {
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
