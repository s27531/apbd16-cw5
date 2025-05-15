namespace CodeFirst.DTOs;

/*
 * Subtype for PrescriptionDto class.
 * Attributes from Prescription class,
 * with patient and doctor details.
 */
public class PrescriptionWithDetailsDto : PrescriptionDto
{
    public required PatientDto Patient { get; init; }
    public required DoctorDto Doctor { get; init; }
}