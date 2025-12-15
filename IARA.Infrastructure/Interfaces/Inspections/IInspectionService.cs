using IARA.DomainModel;
using IARA.DomainModel.DTOs.Inspections;
using IARA.DomainModel.Filters.Inspections;

namespace IARA.Infrastructure.Interfaces.Inspections;

public interface IInspectionService
{
    Task<IEnumerable<InspectionResponseDTO>> GetAllAsync(BaseFilter<InspectionFilter> filters);
    Task<InspectionResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(InspectionRequestDTO inspection);
    Task<bool> EditAsync(InspectionRequestDTO inspection);
    Task<bool> DeleteAsync(int id);
    Task<bool> CompleteInspectionAsync(int id);
}
