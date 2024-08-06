namespace PatientAdministrationSystem.Application.DTOs
{
    public class PatientVisitDto
    {
        public Guid VisitId { get; set; }
        public DateTime Date { get; set; }
        public string HospitalName { get; set; }
        public string HospitalAddress { get; set; }
    }

}
