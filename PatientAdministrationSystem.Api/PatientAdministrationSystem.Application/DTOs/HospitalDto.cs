namespace PatientAdministrationSystem.Application.DTOs
{
    public class HospitalDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}
