using CodeFirst.DTOs;

namespace CodeFirst.Services;

public interface IDbService
{
    Task<PatientWithPrescriptionDto> GetPatient(int patientId);
    Task<int?> AddPrescription(PrescriptionWithDetailsDto prescription);
}