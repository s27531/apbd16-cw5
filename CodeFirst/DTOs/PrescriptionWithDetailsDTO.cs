namespace CodeFirst.DTOs;

public class PrescriptionWithDetailsDTO
{
    public PatientDTO Patient { get; set; }
    public DoctorDTO Doctor { get; set; }
    public List<MedicamentDTO> Medicaments { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
}