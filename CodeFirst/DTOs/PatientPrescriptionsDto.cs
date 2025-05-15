namespace CodeFirst.DTOs;

/*
 * Subtype for PrescriptionDto class.
 * Attributes from Prescription class,
 * with id of the prescription and doctor details.
 */
public class PatientPrescriptionsDto : PrescriptionDto
{
    public required int IdPrescription { get; init; }
    public required DoctorDto Doctor { get; init; }
}