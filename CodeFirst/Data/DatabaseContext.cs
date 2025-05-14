using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Data;

public class DatabaseContext : DbContext
{
    
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>().HasData(new List<Doctor>
        {
            new() { IdDoctor = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jan.kowalski@nfz.gov.pl" },
            new() { IdDoctor = 2, FirstName = "Andrzej", LastName = "Nowak", Email = "andrzej.nowak@nfz.gov.pl" }
        });

        modelBuilder.Entity<Patient>().HasData(new List<Patient>
        {
            new() { IdPatient = 1, FirstName = "Wojciech", LastName = "Nowak", Birthdate = DateOnly.Parse("1995-05-12") },
            new() { IdPatient = 2, FirstName = "Mariusz", LastName = "Kowalski", Birthdate = DateOnly.Parse("1997-02-26") }
        });

        modelBuilder.Entity<Prescription>().HasData(new List<Prescription>
        {
            new() { IdPrescription = 1, Date = DateOnly.Parse("2025-05-14"), DueDate = DateOnly.Parse("2025-05-24"), IdPatient = 1, IdDoctor = 2 },
            new() { IdPrescription = 2, Date = DateOnly.Parse("2025-05-07"), DueDate = DateOnly.Parse("2025-05-24"), IdPatient = 2, IdDoctor = 1 }
        });

        modelBuilder.Entity<Medicament>().HasData(new List<Medicament>
        {
            new() { IdMedicament = 1, Name = "Medicament Uno", Description = "Very stronk medicament", Type = "Type Uno" },
            new() { IdMedicament = 2, Name = "Medicament Dos", Description = "Very weak medicament", Type = "Type Dos" },
        });

        modelBuilder.Entity<PrescriptionMedicament>().HasData(new List<PrescriptionMedicament>
        {
            new() { IdMedicament = 1, IdPrescription = 2, Dose = 2, Details = "Take doses in the morning." },
            new() { IdMedicament = 2, IdPrescription = 1, Dose = 1, Details = "Take doses after dinner." },
        });
    }
}