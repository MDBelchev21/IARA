using IARA.DomainModel;
using IARA.DomainModel.DTOs.Registry;
using IARA.DomainModel.Filters.Registry;
using IARA.Infrastructure.Interfaces.Registry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IARA.API.Controllers.Registry;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "Administrator")]
public class QualificationController : ControllerBase
{
    private readonly IQualificationService _service;
    public QualificationController(IQualificationService service) { _service = service; }

    [HttpPost("getall")]
    public async Task<ActionResult<IEnumerable<QualificationResponseDTO>>> GetAll([FromBody] BaseFilter<PersonFilter> filters)
    {
        var result = await _service.GetAllAsync(filters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<QualificationResponseDTO>> Get(int id)
    {
        var result = await _service.GetAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Add([FromBody] QualificationRequestDTO request)
    {
        var id = await _service.AddAsync(request);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut]
    public async Task<ActionResult<bool>> Edit([FromBody] QualificationRequestDTO request)
    {
        var result = await _service.EditAsync(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return Ok(result);
    }
}

