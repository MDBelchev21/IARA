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
public class InspectionController : ControllerBase
{
    private readonly IInspectionService _inspectionService;

    public InspectionController(IInspectionService inspectionService)
    {
        _inspectionService = inspectionService;
    }

    [HttpPost("getall")]
    public async Task<ActionResult<IEnumerable<InspectionResponseDTO>>> GetAll([FromBody] BaseFilter<InspectionFilter> filters)
    {
        var result = await _inspectionService.GetAllAsync(filters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InspectionResponseDTO>> Get(int id)
    {
        var result = await _inspectionService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Add([FromBody] InspectionRequestDTO inspection)
    {
        var id = await _inspectionService.AddAsync(inspection);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut]
    public async Task<ActionResult<bool>> Edit([FromBody] InspectionRequestDTO inspection)
    {
        var result = await _inspectionService.EditAsync(inspection);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var result = await _inspectionService.DeleteAsync(id);
        return Ok(result);
    }

    [HttpPost("{id}/complete")]
    public async Task<ActionResult<bool>> CompleteInspection(int id)
    {
        var result = await _inspectionService.CompleteInspectionAsync(id);
        return Ok(result);
    }
}
