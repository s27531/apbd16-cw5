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
    
    public async Task<bool> AddPrescription(PrescriptionWithDetailsDto prescription)
    {
        var patient = await context.Patients
            .SingleOrDefaultAsync(p => p.IdPatient == prescription.Patient.IdPatient);

        // Add patient if not exist.
        if (patient == null)
        {
            patient = new Patient()
            {
                IdPatient = prescription.Patient.IdPatient,
                FirstName = prescription.Patient.FirstName,
                LastName = prescription.Patient.LastName,
                Birthdate = prescription.Patient.Birthdate
            };
            context.Patients.Add(patient);
        }

        // Validate medicaments existence
        var medicamentIds = prescription.Medicaments.Select(m => m.IdMedicament).ToList();
        var existingIds = await context.Medicaments
            .Where(m => medicamentIds.Contains(m.IdMedicament))
            .Select(m => m.IdMedicament)
            .ToListAsync();
        if (existingIds.Count != medicamentIds.Count)
        {
            return false;
        }
        
        // Validate medicament count
        if (prescription.Medicaments.Count > 10)
        {
            return false;
        }
        
        // Validate doctor's existence
        if (!await context.Doctors.AnyAsync(d => d.IdDoctor == prescription.Doctor.IdDoctor))
        {
            return false;
        }
        
        // Validate dates
        if (prescription.Date > prescription.DueDate)
        {
            return false;
        }

        // Create new prescription
        var createdPrescription = new Prescription
        {
            Date = prescription.Date,
            DueDate = prescription.DueDate,
            IdPatient = prescription.Patient.IdPatient,
            IdDoctor = prescription.Doctor.IdDoctor
        };
        context.Prescriptions.Add(createdPrescription);
        await context.SaveChangesAsync(); // Attaching id for prescription

        // Add medicaments for prescription
        var prescriptionMedicaments = prescription.Medicaments.Select(m => new PrescriptionMedicament
        {
            IdPrescription = createdPrescription.IdPrescription,
            IdMedicament = m.IdMedicament,
            Dose = m.Dose,
            Details = m.Details
        }).ToList();
        
        context.PrescriptionMedicaments.AddRange(prescriptionMedicaments);
        await context.SaveChangesAsync();

        return true;
    }
}