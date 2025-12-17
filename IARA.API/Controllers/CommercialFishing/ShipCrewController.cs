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
public class ShipCrewController : ControllerBase
{
    private readonly IShipCrewService _service;
    public ShipCrewController(IShipCrewService service) { _service = service; }

    [HttpPost("getall")]
    [Authorize(Policy = "Inspector")]
    public async Task<ActionResult<IEnumerable<ShipCrewResponseDTO>>> GetAll([FromBody] BaseFilter<IARA.DomainModel.Filters.CommercialFishing.ShipCrewFilter> filters)
    {
        var result = await _service.GetAllAsync(filters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Inspector")]
    public async Task<ActionResult<ShipCrewResponseDTO>> Get(int id)
    {
        var result = await _service.GetAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<int>> Add([FromBody] ShipCrewRequestDTO request)
    {
        var id = await _service.AddAsync(request);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<bool>> Edit([FromBody] ShipCrewRequestDTO request)
    {
        var result = await _service.EditAsync(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return Ok(result);
    }
}
