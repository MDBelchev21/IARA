using IARA.DomainModel;
using IARA.DomainModel.DTOs.RecreationalFishing;
using IARA.DomainModel.Filters.RecreationalFishing;

namespace IARA.Infrastructure.Interfaces.RecreationalFishing;

public interface IRecreationalTicketService
{
    Task<IEnumerable<RecreationalTicketResponseDTO>> GetAllAsync(BaseFilter<RecreationalTicketFilter> filters);
    Task<RecreationalTicketResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(RecreationalTicketRequestDTO ticket);
    Task<bool> EditAsync(RecreationalTicketRequestDTO ticket);
    Task<bool> DeleteAsync(int id);
    Task<bool> DeactivateTicketAsync(int id);
}



