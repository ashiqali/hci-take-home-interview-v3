using Microsoft.AspNetCore.Mvc;
using PatientAdministrationSystem.Application.DTOs;
using PatientAdministrationSystem.Application.Interfaces;

namespace PatientAdministrationSystem.API.Controllers
{
    [Route("api/patients")]
    [ApiExplorerSettings(GroupName = "Patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientsService _patientsService;

        public PatientsController(IPatientsService patientsService)
        {
            _patientsService = patientsService;
        }

        /// <summary>
        /// Get all patients.
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get a patient by ID.
        /// </summary>
        /// <param name="id">The ID of the patient</param>
        /// <returns>A patient</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(Guid id)
        {
            try
            {
                var patient = await _patientsService.GetPatientByIdAsync(id);
                if (patient == null)
                {
                    return NotFound();
                }
                return Ok(patient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get visits for a specific patient.
        /// </summary>
        /// <param name="id">The ID of the patient</param>
        /// <returns>A list of visits for the patient</returns>
        [HttpGet("{id}/visits")]
        public async Task<IActionResult> GetPatientVisits(Guid id)
        {
            try
            {
                var visits = await _patientsService.GetPatientVisitsAsync(id);
                if (visits == null || !visits.Any())
                {
                    return NotFound();
                }
                return Ok(visits);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

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

            try
            {
                await _patientsService.CreatePatientAsync(patientDto);
                return CreatedAtAction(nameof(GetPatientById), new { id = patientDto.Id }, patientDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing patient.
        /// </summary>
        /// <param name="id">The ID of the patient</param>
        /// <param name="patientDto">The updated patient data</param>
        /// <returns>Action result</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] PatientDto patientDto)
        {
            if (patientDto == null)
            {
                return BadRequest("Patient data is null.");
            }

            try
            {
                await _patientsService.UpdatePatientAsync(id, patientDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a patient.
        /// </summary>
        /// <param name="id">The ID of the patient</param>
        /// <returns>Action result</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            try
            {
                await _patientsService.DeletePatientAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
