using CodeFirst.DTOs;
using CodeFirst.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionsController : ControllerBase
{
    private readonly IDbService _dbService;

    public PrescriptionsController(IDbService dbService)
    {
        _dbService = dbService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] PrescriptionWithDetailsDTO prescription)
    {
        var isAdded = await _dbService.AddPrescription(prescription);
        if (isAdded)
        {
            return Created();
        }

        return BadRequest();
    }
}