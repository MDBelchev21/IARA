using IARA.DomainModel;
using IARA.DomainModel.DTOs.RecreationalFishing;
using IARA.DomainModel.Filters.RecreationalFishing;

namespace IARA.Infrastructure.Interfaces.RecreationalFishing;

public interface IRecreationalTicketTypeService
{
    Task<IEnumerable<RecreationalTicketTypeResponseDTO>> GetAllAsync(BaseFilter<RecreationalTicketTypeFilter> filters);
    Task<RecreationalTicketTypeResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(RecreationalTicketTypeRequestDTO type);
    Task<bool> EditAsync(RecreationalTicketTypeRequestDTO type);
    Task<bool> DeleteAsync(int id);
}

