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
public class TransportLineController : ControllerBase
{
    private readonly ITransportLineService _service;

    public TransportLineController(ITransportLineService service)
    {
        _service = service;
    }

    [HttpPost("getall")]
    [Authorize(Policy = "Inspector")]
    public async Task<ActionResult<IEnumerable<TransportLineResponseDTO>>> GetAll([FromBody] BaseFilter<TransportLineFilter> filters)
    {
        var result = await _service.GetAllAsync(filters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Inspector")]
    public async Task<ActionResult<TransportLineResponseDTO>> Get(int id)
    {
        var result = await _service.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "ShipOwner")]
    public async Task<ActionResult<int>> Add([FromBody] TransportLineRequestDTO dto)
    {
        var id = await _service.AddAsync(dto);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut]
    [Authorize(Policy = "ShipOwner")]
    public async Task<ActionResult<bool>> Edit([FromBody] TransportLineRequestDTO dto)
    {
        var success = await _service.EditAsync(dto);
        return Ok(success);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        return Ok(success);
    }
}

