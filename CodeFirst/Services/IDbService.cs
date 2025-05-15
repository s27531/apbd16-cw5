using CodeFirst.DTOs;

namespace CodeFirst.Services;

public interface IDbService
{
    Task<PatientWithPrescriptionDto> GetPatient(int patientId);
    Task<bool> AddPrescription(PrescriptionWithDetailsDto prescription);
}