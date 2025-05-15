namespace CodeFirst.DTOs;

public class PatientPrescriptionsDTO
{
    public int IdPrescription { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
    public List<MedicamentDTO> Medicaments { get; set; }
    public DoctorDTO Doctor { get; set; }
}