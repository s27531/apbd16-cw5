namespace CodeFirst.DTOs;

public class PatientWithDetailsDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly Birthdate { get; set; }
    public List<PatientPrescriptionsDTO> Prescriptions { get; set; }
}