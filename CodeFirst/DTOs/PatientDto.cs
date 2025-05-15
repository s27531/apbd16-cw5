namespace CodeFirst.DTOs;

/*
 * 1:1 attribs with Patient class.
 */
public class PatientDto
{
    public int IdPatient { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required DateOnly Birthdate { get; init; }
}