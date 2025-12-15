using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;
using IARA.Infrastructure.Interfaces.CommercialFishing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IARA.API.Controllers.CommercialFishing;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FishingPermitController : ControllerBase
{
    private readonly IFishingPermitService _permitService;

    public FishingPermitController(IFishingPermitService permitService)
    {
        _permitService = permitService;
    }

    [HttpPost("getall")]
    [Authorize(Policy = "Inspector")]
    public async Task<ActionResult<IEnumerable<FishingPermitResponseDTO>>> GetAll([FromBody] BaseFilter<FishingPermitFilter> filters)
    {
        var result = await _permitService.GetAllAsync(filters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Inspector")]
    public async Task<ActionResult<FishingPermitResponseDTO>> Get(int id)
    {
        var result = await _permitService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<int>> Add([FromBody] FishingPermitRequestDTO permit)
    {
        var id = await _permitService.AddAsync(permit);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<bool>> Edit([FromBody] FishingPermitRequestDTO permit)
    {
        var result = await _permitService.EditAsync(permit);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var result = await _permitService.DeleteAsync(id);
        return Ok(result);
    }

    [HttpPost("{id}/revoke")]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<bool>> RevokePermit(int id)
    {
        var result = await _permitService.RevokePermitAsync(id);
        return Ok(result);
    }

    [HttpGet("{id}/isvalid")]
    [Authorize(Policy = "Inspector")]
    public async Task<ActionResult<bool>> IsPermitValid(int id)
    {
        var result = await _permitService.IsPermitValidAsync(id);
        return Ok(result);
    }
}
