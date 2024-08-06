namespace PatientAdministrationSystem.Application.DTOs
{
    public class PatientDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
