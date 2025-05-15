namespace CodeFirst.DTOs;

public class PatientPrescriptionsDTO
{
    public int IdPrescription { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
    public List<MedicamentWithDetailsDto> Medicaments { get; set; }
    public DoctorDto Doctor { get; set; }
}