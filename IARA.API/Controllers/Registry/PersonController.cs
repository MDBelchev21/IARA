using IARA.DomainModel;
using IARA.DomainModel.DTOs.Registry;
using IARA.DomainModel.Filters.Registry;
using IARA.Infrastructure.Interfaces.Registry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IARA.API.Controllers.Registry;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpPost("getall")]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<IEnumerable<PersonResponseDTO>>> GetAll([FromBody] BaseFilter<PersonFilter> filters)
    {
        var result = await _personService.GetAllAsync(filters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<PersonResponseDTO>> Get(int id)
    {
        var result = await _personService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<int>> Add([FromBody] PersonRequestDTO person)
    {
        var id = await _personService.AddAsync(person);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<bool>> Edit([FromBody] PersonRequestDTO person)
    {
        var result = await _personService.EditAsync(person);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Administrator")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var result = await _personService.DeleteAsync(id);
        return Ok(result);
    }
}

