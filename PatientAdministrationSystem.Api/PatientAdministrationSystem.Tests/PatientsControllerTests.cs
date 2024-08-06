using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PatientAdministrationSystem.API.Controllers;
using PatientAdministrationSystem.Application.DTOs;
using PatientAdministrationSystem.Application.Interfaces;

namespace PatientAdministrationSystem.Tests
{
    public class PatientsControllerTests
    {
        private readonly Mock<IPatientsService> _mockPatientsService;
        private readonly Mock<ILogger<PatientsController>> _mockLogger;
        private readonly PatientsController _controller;

        public PatientsControllerTests()
        {
            _mockPatientsService = new Mock<IPatientsService>();
            _mockLogger = new Mock<ILogger<PatientsController>>();
            _controller = new PatientsController(_mockPatientsService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllPatients_ShouldReturnOkResult_WhenPatientsExist()
        {
            // Arrange
            var patients = new List<PatientDto>
            {
                new PatientDto { Id = Guid.NewGuid(), FirstName = "AsiqAli",LastName="Ziaudeen" },
                new PatientDto { Id = Guid.NewGuid(), FirstName = "Jane", LastName="Doe" }
            };
            _mockPatientsService.Setup(service => service.GetAllPatientsAsync()).ReturnsAsync(patients);

            // Act
            var result = await _controller.GetAllPatients();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(patients);
        }

        [Fact]
        public async Task GetAllPatients_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _mockPatientsService.Setup(service => service.GetAllPatientsAsync()).ThrowsAsync(new Exception("Error"));

            // Act
            var result = await _controller.GetAllPatients();

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult.StatusCode.Should().Be(500);
        }

        // Additional tests for other methods...
    }
}
