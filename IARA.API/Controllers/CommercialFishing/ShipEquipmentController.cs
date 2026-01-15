using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;
using IARA.Infrastructure.Interfaces.CommercialFishing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IARA.API.Controllers.CommercialFishing;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class ShipEquipmentController : ControllerBase
{
    private readonly IShipEquipmentService _shipEquipmentService;

    public ShipEquipmentController(IShipEquipmentService shipEquipmentService)
    {
        _shipEquipmentService = shipEquipmentService;
    }

    [HttpPost]
    [Authorize(Policy = "ShipOwnerOrInspector")]
    public async Task<ActionResult<IEnumerable<ShipEquipmentResponseDTO>>> GetAll([FromBody] BaseFilter<ShipEquipmentFilter> filters)
    {
        var result = await _shipEquipmentService.GetAllAsync(filters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "ShipOwnerOrInspector")]
    public async Task<ActionResult<ShipEquipmentResponseDTO>> Get(int id)
    {
        var result = await _shipEquipmentService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "ShipOwner")]
    public async Task<ActionResult<int>> Add([FromBody] ShipEquipmentRequestDTO equipment)
    {
        var id = await _shipEquipmentService.AddAsync(equipment);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut]
    [Authorize(Policy = "ShipOwner")]
    public async Task<ActionResult<bool>> Edit([FromBody] ShipEquipmentRequestDTO equipment)
    {
        var result = await _shipEquipmentService.EditAsync(equipment);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "ShipOwner")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var result = await _shipEquipmentService.DeleteAsync(id);
        return Ok(result);
    }
}
