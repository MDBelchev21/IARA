using IARA.DomainModel;
using IARA.DomainModel.DTOs.Registry;
using IARA.DomainModel.Filters.Registry;
using IARA.Infrastructure.Interfaces.Registry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IARA.API.Controllers.Registry;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Policy = "Administrator")]
public class ShipOwnerController : ControllerBase
{
    private readonly IShipOwnerService _shipOwnerService;

    public ShipOwnerController(IShipOwnerService shipOwnerService)
    {
        _shipOwnerService = shipOwnerService;
    }

    [HttpPost("getall")]
    public async Task<ActionResult<IEnumerable<ShipOwnerResponseDTO>>> GetAll([FromBody] BaseFilter<PersonFilter> filters)
    {
        var result = await _shipOwnerService.GetAllAsync(filters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShipOwnerResponseDTO>> Get(int id)
    {
        var result = await _shipOwnerService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Add([FromBody] ShipOwnerRequestDTO owner)
    {
        var id = await _shipOwnerService.AddAsync(owner);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut]
    public async Task<ActionResult<bool>> Edit([FromBody] ShipOwnerRequestDTO owner)
    {
        var result = await _shipOwnerService.EditAsync(owner);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var result = await _shipOwnerService.DeleteAsync(id);
        return Ok(result);
    }
}
