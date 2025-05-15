using CodeFirst.Data;
using CodeFirst.DTOs;
using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Services;

public class DbService(DatabaseContext context) : IDbService
{
    public async Task<PatientWithPrescriptionDto> GetPatient(int patientId)
    {
        var patientWithDetails = await context.Patients
            .Where(p => p.IdPatient == patientId)
            .Select(p => new PatientWithPrescriptionDto
            {
                IdPatient = p.IdPatient,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Birthdate = p.Birthdate,
                Prescriptions = p.Prescriptions.Select(pr => new PatientPrescriptionsDto
                {
                    IdPrescription = pr.IdPrescription,
                    Date = pr.Date,
                    DueDate = pr.DueDate,
                    Doctor = new DoctorDto
                    {
                        IdDoctor = pr.Doctor.IdDoctor,
                        FirstName = pr.Doctor.FirstName,
                        LastName = pr.Doctor.LastName,
                        Email = pr.Doctor.Email
                    },
                    Medicaments = pr.PrescriptionMedicaments.Select(pm => new MedicamentWithDetailsDto
                    {
                        IdMedicament = pm.Medicament.IdMedicament,
                        Name = pm.Medicament.Name,
                        Description = pm.Medicament.Description,
                        Type = pm.Medicament.Type,
                        Details = pm.Details,
                        Dose = pm.Dose
                    }).ToList()
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (patientWithDetails == null)
        {
            throw new KeyNotFoundException($"Patient with Id {patientId} not found.");
        }
        
        return patientWithDetails;
    }
    
    public async Task<int?> AddPrescription(PrescriptionWithDetailsDto prescription)
    { // Returns id of newly created prescription, or null.
        // Validate medicament count
        if (prescription.Medicaments.Count > 10)
        {
            return null;
        }
        
        // Validate dates
        if (prescription.Date > prescription.DueDate)
        {
            return null;
        }
        
        // Validate medicament existence
        var medicamentIds = prescription.Medicaments.Select(m => m.IdMedicament).ToList();
        var existingMedicamentIds = await context.Medicaments
            .Where(m => medicamentIds.Contains(m.IdMedicament))
            .Select(m => m.IdMedicament)
            .ToListAsync();
        if (existingMedicamentIds.Count != medicamentIds.Count)
        {
            return null;
        }
        
        // Validate patient, and add it if not exists.
        var patient = await context.Patients
            .FirstOrDefaultAsync(p => p.FirstName == prescription.Patient.FirstName &&
                                       p.LastName == prescription.Patient.LastName &&
                                       p.Birthdate == prescription.Patient.Birthdate);
        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = prescription.Patient.FirstName,
                LastName = prescription.Patient.LastName,
                Birthdate = prescription.Patient.Birthdate
            };

            context.Patients.Add(patient);
            await context.SaveChangesAsync();
        }
        
        // Create prescription
        var newPrescription = new Prescription
        {
            Date = prescription.Date,
            DueDate = prescription.DueDate,
            IdPatient = patient.IdPatient,
            IdDoctor = prescription.Doctor.IdDoctor
        };
        context.Prescriptions.Add(newPrescription);
        await context.SaveChangesAsync(); // Crucial for getting the id of newly create prescription
        
        // Add medicaments for prescription
        var prescriptionMedicaments = prescription.Medicaments.Select(m => new PrescriptionMedicament
        {
            IdPrescription = newPrescription.IdPrescription,
            IdMedicament = m.IdMedicament,
            Dose = m.Dose,
            Details = m.Details
        }).ToList();
        context.PrescriptionMedicaments.AddRange(prescriptionMedicaments);
        await context.SaveChangesAsync();
        
        return newPrescription.IdPrescription;
    }
}