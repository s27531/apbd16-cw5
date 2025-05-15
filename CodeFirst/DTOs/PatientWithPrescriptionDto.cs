namespace CodeFirst.DTOs;

/*
 * Subtype of PatientDto.
 * Adds additional prescription details.
 */
public class PatientWithPrescriptionDto : PatientDto
{ 
    public required List<PatientPrescriptionsDto> Prescriptions { get; init; }
}