using IARA.DomainModel;
using IARA.DomainModel.DTOs.Inspections;
using IARA.DomainModel.Filters.Inspections;

namespace IARA.Infrastructure.Interfaces.Inspections;

public interface IViolationService
{
    Task<IEnumerable<ViolationResponseDTO>> GetAllAsync(BaseFilter<ViolationFilter> filters);
    Task<ViolationResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(ViolationRequestDTO violation);
    Task<bool> EditAsync(ViolationRequestDTO violation);
    Task<bool> DeleteAsync(int id);
    Task<bool> MarkAsPaidAsync(int id);
    Task<bool> IssueFineAsync(int id, decimal amount);
}
