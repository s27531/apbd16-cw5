using CodeFirst.DTOs;

namespace CodeFirst.Services;

public interface IDbService
{
    Task<bool> AddPrescription(PrescriptionWithDetailsDTO prescription);
}