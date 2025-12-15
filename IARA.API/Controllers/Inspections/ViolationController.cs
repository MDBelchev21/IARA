using IARA.DomainModel;
using IARA.DomainModel.DTOs.Inspections;
using IARA.DomainModel.Filters.Inspections;
using IARA.Infrastructure.Interfaces.Inspections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IARA.API.Controllers.Inspections;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "Inspector")]
public class ViolationController : ControllerBase
{
    private readonly IViolationService _violationService;

    public ViolationController(IViolationService violationService)
    {
        _violationService = violationService;
    }

    [HttpPost("getall")]
    public async Task<ActionResult<IEnumerable<ViolationResponseDTO>>> GetAll([FromBody] BaseFilter<ViolationFilter> filters)
    {
        var result = await _violationService.GetAllAsync(filters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ViolationResponseDTO>> Get(int id)
    {
        var result = await _violationService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Add([FromBody] ViolationRequestDTO violation)
    {
        var id = await _violationService.AddAsync(violation);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut]
    public async Task<ActionResult<bool>> Edit([FromBody] ViolationRequestDTO violation)
    {
        var result = await _violationService.EditAsync(violation);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var result = await _violationService.DeleteAsync(id);
        return Ok(result);
    }

    [HttpPost("{id}/issuefine")]
    public async Task<ActionResult<bool>> IssueFine(int id, [FromQuery] decimal amount)
    {
        var result = await _violationService.IssueFineAsync(id, amount);
        return Ok(result);
    }
}
