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
public class RecreationalFishermanController : ControllerBase
{
    private readonly IRecreationalFishermanService _fishermanService;

    public RecreationalFishermanController(IRecreationalFishermanService fishermanService)
    {
        _fishermanService = fishermanService;
    }

    [HttpPost("getall")]
    [Authorize(Policy = "Inspector")]
    public async Task<ActionResult<IEnumerable<RecreationalFishermanResponseDTO>>> GetAll([FromBody] BaseFilter<RecreationalFishermanFilter> filters)
    {
        var result = await _fishermanService.GetAllAsync(filters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Inspector")]
    public async Task<ActionResult<RecreationalFishermanResponseDTO>> Get(int id)
    {
        var result = await _fishermanService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<int>> Add([FromBody] RecreationalFishermanRequestDTO fisherman)
    {
        var id = await _fishermanService.AddAsync(fisherman);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut]
    [Authorize(Policy = "RecreationalFisherman")]
    public async Task<ActionResult<bool>> Edit([FromBody] RecreationalFishermanRequestDTO fisherman)
    {
        var result = await _fishermanService.EditAsync(fisherman);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var result = await _fishermanService.DeleteAsync(id);
        return Ok(result);
    }
}
