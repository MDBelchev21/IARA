using IARA.DomainModel;
using IARA.DomainModel.DTOs.RecreationalFishing;
using IARA.DomainModel.Filters.RecreationalFishing;
using IARA.Infrastructure.Interfaces.RecreationalFishing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IARA.API.Controllers.RecreationalFishing;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class RecreationalCatchController : ControllerBase
{
    private readonly IRecreationalCatchService _catchService;
    public RecreationalCatchController(IRecreationalCatchService catchService) { _catchService = catchService; }

    [HttpPost("getall")]
    [Authorize(Policy = "Inspector")]
    public async Task<ActionResult<IEnumerable<RecreationalCatchResponseDTO>>> GetAll([FromBody] BaseFilter<RecreationalCatchFilter> filters)
    {
        var result = await _catchService.GetAllAsync(filters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Inspector")]
    public async Task<ActionResult<RecreationalCatchResponseDTO>> Get(int id)
    {
        var result = await _catchService.GetAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "RecreationalFisherman")]
    public async Task<ActionResult<int>> Add([FromBody] RecreationalCatchRequestDTO request)
    {
        var id = await _catchService.AddAsync(request);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut]
    [Authorize(Policy = "RecreationalFisherman")]
    public async Task<ActionResult<bool>> Edit([FromBody] RecreationalCatchRequestDTO request)
    {
        var result = await _catchService.EditAsync(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var result = await _catchService.DeleteAsync(id);
        return Ok(result);
    }
}
