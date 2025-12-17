using IARA.DomainModel;
using IARA.DomainModel.DTOs.RecreationalFishing;
using IARA.DomainModel.Filters.RecreationalFishing;
using IARA.Infrastructure.Interfaces.RecreationalFishing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IARA.API.Controllers.RecreationalFishing;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Policy = "Administrator")]
public class RecreationalTicketTypeController : ControllerBase
{
    private readonly IRecreationalTicketTypeService _service;
    public RecreationalTicketTypeController(IRecreationalTicketTypeService service) { _service = service; }

    [HttpPost("getall")]
    public async Task<ActionResult<IEnumerable<RecreationalTicketTypeResponseDTO>>> GetAll([FromBody] BaseFilter<RecreationalTicketTypeFilter> filters)
    {
        var result = await _service.GetAllAsync(filters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RecreationalTicketTypeResponseDTO>> Get(int id)
    {
        var result = await _service.GetAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Add([FromBody] RecreationalTicketTypeRequestDTO request)
    {
        var id = await _service.AddAsync(request);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut]
    public async Task<ActionResult<bool>> Edit([FromBody] RecreationalTicketTypeRequestDTO request)
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
