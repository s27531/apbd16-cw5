using CodeFirst.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController(IDbService dbService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientById(int id)
    {
        try
        {
            var patient = await dbService.GetPatient(id);
            return Ok(patient);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}