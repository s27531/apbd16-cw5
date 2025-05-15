using CodeFirst.DTOs;
using CodeFirst.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionsController(IDbService dbService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] PrescriptionWithDetailsDto prescription)
    {
        if (await dbService.AddPrescription(prescription))
        {
            return Created();
        }
        return BadRequest();
    }
}