using CodeFirst.Data;
using CodeFirst.DTOs;
using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<PatientWithDetailsDTO> GetPatient(int patientId)
    {
        var patientWithDetails = await _context.Patients
            .Where(p => p.IdPatient == patientId)
            .Select(p => new PatientWithDetailsDTO
            {
                IdPatient = p.IdPatient,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Birthdate = p.Birthdate,
                Prescriptions = p.Prescriptions.Select(pr => new PatientPrescriptionsDTO
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
                    Medicaments = pr.PrescriptionMedicaments.Select(pm => new MedicamentDTO
                    {
                        IdMedicament = pm.Medicament.IdMedicament,
                        Name = pm.Medicament.Name,
                        Description = pm.Medicament.Description,
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
    
    public async Task<bool> AddPrescription(PrescriptionWithDetailsDTO prescription)
    {
        var patient = await _context.Patients
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
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        // Validate medicaments existence
        var isMedicamentsValid = prescription.Medicaments
            .Where(m => !_context.Medicaments.Any(m1 => m1.IdMedicament == m.IdMedicament))
            .ToList().Any();
        if (isMedicamentsValid)
        {
            return false;
        }
        
        // Validate medicament count
        if (prescription.Medicaments.Count > 10)
        {
            return false;
        }
        
        // Validate doctor's existence
        if (!_context.Doctors.Any(d => d.IdDoctor == prescription.Doctor.IdDoctor))
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
        _context.Prescriptions.Add(createdPrescription);
        await _context.SaveChangesAsync();

        // Add medicaments for prescription
        var prescriptionMedicaments = prescription.Medicaments.Select(m => new PrescriptionMedicament
        {
            IdPrescription = createdPrescription.IdPrescription,
            IdMedicament = m.IdMedicament,
            Dose = m.Dose,
            Details = m.Details
        }).ToList();
        
        _context.PrescriptionMedicaments.AddRange(prescriptionMedicaments);
        await _context.SaveChangesAsync();

        return true;
    }
}