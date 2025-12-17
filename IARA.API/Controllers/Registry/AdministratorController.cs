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
public class AdministratorController : ControllerBase
{
    private readonly IAdministratorService _administratorService;

    public AdministratorController(IAdministratorService administratorService)
    {
        _administratorService = administratorService;
    }

    [HttpPost("getall")]
    public async Task<ActionResult<IEnumerable<PersonResponseDTO>>> GetAll([FromBody] BaseFilter<PersonFilter> filters)
    {
        var result = await _administratorService.GetAllAsync(filters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PersonResponseDTO>> Get(int id)
    {
        var result = await _administratorService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Add([FromBody] PersonRequestDTO person)
    {
        var id = await _administratorService.AddAsync(person);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var result = await _administratorService.DeleteAsync(id);
        return Ok(result);
    }
}
