using IARA.DomainModel;
using IARA.DomainModel.DTOs.RecreationalFishing;
using IARA.DomainModel.Filters.RecreationalFishing;
using IARA.Infrastructure.Interfaces.RecreationalFishing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IARA.API.Controllers.RecreationalFishing;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RecreationalTicketController : ControllerBase
{
    private readonly IRecreationalTicketService _ticketService;

    public RecreationalTicketController(IRecreationalTicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpPost("getall")]
    [Authorize(Policy = "Inspector")]
    public async Task<ActionResult<IEnumerable<RecreationalTicketResponseDTO>>> GetAll([FromBody] BaseFilter<RecreationalTicketFilter> filters)
    {
        var result = await _ticketService.GetAllAsync(filters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Inspector")]
    public async Task<ActionResult<RecreationalTicketResponseDTO>> Get(int id)
    {
        var result = await _ticketService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "RecreationalFisherman")]
    public async Task<ActionResult<int>> Add([FromBody] RecreationalTicketRequestDTO ticket)
    {
        var id = await _ticketService.AddAsync(ticket);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<bool>> Edit([FromBody] RecreationalTicketRequestDTO ticket)
    {
        var result = await _ticketService.EditAsync(ticket);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var result = await _ticketService.DeleteAsync(id);
        return Ok(result);
    }

    [HttpPost("{id}/deactivate")]
    [Authorize(Policy = "Inspector")]
    public async Task<ActionResult<bool>> DeactivateTicket(int id)
    {
        var result = await _ticketService.DeactivateTicketAsync(id);
        return Ok(result);
    }
}
