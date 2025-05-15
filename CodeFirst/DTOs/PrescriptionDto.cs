namespace CodeFirst.DTOs;

/*
 * Attribs from Prescription class, without id,
 * with MedicamentWithDetails collection.
 */
public class PrescriptionDto
{
    public required DateOnly Date { get; init; }
    public required DateOnly DueDate { get; init; }
    public required List<MedicamentWithDetailsDto> Medicaments { get; init; }
}