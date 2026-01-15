using IARA.DomainModel;
using IARA.DomainModel.DTOs.RecreationalFishing;
using IARA.DomainModel.Filters.RecreationalFishing;

namespace IARA.Infrastructure.Interfaces.RecreationalFishing;

public interface IRecreationalFishermanService
{
    Task<IEnumerable<RecreationalFishermanResponseDTO>> GetAllAsync(BaseFilter<RecreationalFishermanFilter> filters);
    Task<RecreationalFishermanResponseDTO?> GetAsync(int id);
    Task<int?> GetByPersonIdAsync(int personId);
    Task<int> AddAsync(RecreationalFishermanRequestDTO fisherman);
    Task<bool> EditAsync(RecreationalFishermanRequestDTO fisherman);
    Task<bool> DeleteAsync(int id);
}

