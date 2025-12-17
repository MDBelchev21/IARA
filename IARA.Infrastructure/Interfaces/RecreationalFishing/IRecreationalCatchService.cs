using IARA.DomainModel;
using IARA.DomainModel.DTOs.RecreationalFishing;
using IARA.DomainModel.Filters.RecreationalFishing;

namespace IARA.Infrastructure.Interfaces.RecreationalFishing;

public interface IRecreationalCatchService
{
    Task<IEnumerable<RecreationalCatchResponseDTO>> GetAllAsync(BaseFilter<RecreationalCatchFilter> filters);
    Task<RecreationalCatchResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(RecreationalCatchRequestDTO catchDto);
    Task<bool> EditAsync(RecreationalCatchRequestDTO catchDto);
    Task<bool> DeleteAsync(int id);
}

