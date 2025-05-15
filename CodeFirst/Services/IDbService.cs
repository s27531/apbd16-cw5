using CodeFirst.DTOs;

namespace CodeFirst.Services;

public interface IDbService
{
    Task<PatientWithDetailsDTO> GetPatient(int patientId);
    Task<bool> AddPrescription(PrescriptionWithDetailsDTO prescription);
}