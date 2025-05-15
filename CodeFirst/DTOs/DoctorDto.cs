namespace CodeFirst.DTOs;

/*
 * 1:1 attribs with Doctor class.
 */
public class DoctorDto
{
    public required int IdDoctor { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
}