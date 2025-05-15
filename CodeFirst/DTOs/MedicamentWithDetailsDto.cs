namespace CodeFirst.DTOs;

/*
 * 1:1 attribs from Medicament.
 * + those from PrescriptionMedicament class.
 * ^
 * |
 * +- That's because this Dto is used with prescription only
 */
public class MedicamentWithDetailsDto
{
    public required int IdMedicament { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Type { get; init; }
    public int? Dose { get; init; }
    public required string Details { get; init; }
}