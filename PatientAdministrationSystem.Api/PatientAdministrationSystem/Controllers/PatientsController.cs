using Microsoft.AspNetCore.Mvc;
using PatientAdministrationSystem.Application.DTOs;
using PatientAdministrationSystem.Application.Interfaces;

namespace PatientAdministrationSystem.API.Controllers
{
    [Route("api/v1/patients")]
    [ApiExplorerSettings(GroupName = "Patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        #region Fields

        private readonly IPatientsService _patientsService;
        private readonly ILogger<PatientsController> _logger;

        #endregion

        #region Constructors

        public PatientsController(IPatientsService patientsService, ILogger<PatientsController> logger)
        {
            _patientsService = patientsService;
            _logger = logger;
        }

        #endregion

        #region Get All Patients

        /// <summary>
        /// Get all patients with optional pagination, sorting, and filtering.
        /// </summary>
        /// <returns>A list of patients</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            try
            {
                var patients = await _patientsService.GetAllPatientsAsync();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all patients.");
                return StatusCode(500, "Internal server error.");
            }
        }

        #endregion

        #region Get Patient by ID

        /// <summary>
        /// Get a patient by ID.
        /// </summary>
        /// <param name="id">The ID of the patient</param>
        /// <returns>A patient</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid patient ID.");
            }

            try
            {
                var patient = await _patientsService.GetPatientByIdAsync(id);
                if (patient == null)
                {
                    return NotFound("Patient not found.");
                }
                return Ok(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching patient by ID.");
                return StatusCode(500, "Internal server error.");
            }
        }

        #endregion

        #region Get Patient Visits

        /// <summary>
        /// Get visits for a specific patient.
        /// </summary>
        /// <param name="id">The ID of the patient</param>
        /// <returns>A list of visits for the patient</returns>
        [HttpGet("{id}/visits")]
        public async Task<IActionResult> GetPatientVisits(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid patient ID.");
            }

            try
            {
                var visits = await _patientsService.GetPatientVisitsAsync(id);
                if (visits == null || !visits.Any())
                {
                    return NotFound("No visits found for this patient.");
                }
                return Ok(visits);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching patient visits.");
                return StatusCode(500, "Internal server error.");
            }
        }

        #endregion

        #region Create Patient

        /// <summary>
        /// Create a new patient.
        /// </summary>
        /// <param name="patientDto">The patient data</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] PatientDto patientDto)
        {
            if (patientDto == null)
            {
                return BadRequest("Patient data is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid patient data.");
            }

            try
            {
                await _patientsService.CreatePatientAsync(patientDto);
                return CreatedAtAction(nameof(GetPatientById), new { id = patientDto.Id }, patientDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new patient.");
                return StatusCode(500, "Internal server error.");
            }
        }

        #endregion

        #region Update Patient

        /// <summary>
        /// Update an existing patient.
        /// </summary>
        /// <param name="id">The ID of the patient</param>
        /// <param name="patientDto">The updated patient data</param>
        /// <returns>Action result</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] PatientDto patientDto)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid patient ID.");
            }

            if (patientDto == null)
            {
                return BadRequest("Patient data is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid patient data.");
            }

            try
            {
                var patientExists = await _patientsService.GetPatientByIdAsync(id);
                if (patientExists == null)
                {
                    return NotFound("Patient not found.");
                }

                await _patientsService.UpdatePatientAsync(id, patientDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating patient.");
                return StatusCode(500, "Internal server error.");
            }
        }

        #endregion

        #region Delete Patient

        /// <summary>
        /// Delete a patient.
        /// </summary>
        /// <param name="id">The ID of the patient</param>
        /// <returns>Action result</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid patient ID.");
            }

            try
            {
                var patientExists = await _patientsService.GetPatientByIdAsync(id);
                if (patientExists == null)
                {
                    return NotFound("Patient not found.");
                }

                await _patientsService.DeletePatientAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting patient.");
                return StatusCode(500, "Internal server error.");
            }
        }

        #endregion

        #region Health Check

        /// <summary>
        /// Get the health status of the API.
        /// </summary>
        /// <returns>Health status</returns>
        [HttpGet("health")]
        public IActionResult GetHealthStatus()
        {
            // Simple health check, could be enhanced with more detailed checks
            return Ok(new { Status = "Healthy" });
        }

        #endregion

    }
}
