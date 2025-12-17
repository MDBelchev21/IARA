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
public class LegalEntityController : ControllerBase
{
    private readonly ILegalEntityService _legalEntityService;

    public LegalEntityController(ILegalEntityService legalEntityService)
    {
        _legalEntityService = legalEntityService;
    }

    [HttpPost("getall")]
    public async Task<ActionResult<IEnumerable<LegalEntityResponseDTO>>> GetAll([FromBody] BaseFilter<PersonFilter> filters)
    {
        var result = await _legalEntityService.GetAllAsync(filters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LegalEntityResponseDTO>> Get(int id)
    {
        var result = await _legalEntityService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Add([FromBody] LegalEntityRequestDTO entity)
    {
        var id = await _legalEntityService.AddAsync(entity);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut]
    public async Task<ActionResult<bool>> Edit([FromBody] LegalEntityRequestDTO entity)
    {
        var result = await _legalEntityService.EditAsync(entity);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var result = await _legalEntityService.DeleteAsync(id);
        return Ok(result);
    }
}
