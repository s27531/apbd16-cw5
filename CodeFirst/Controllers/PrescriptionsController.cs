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
        var prescriptionId = await dbService.AddPrescription(prescription);
        if (prescriptionId != null)
        {
            return Ok(prescriptionId);
        }
        return BadRequest();
    }
}