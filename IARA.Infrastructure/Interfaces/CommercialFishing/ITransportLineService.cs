using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;

namespace IARA.Infrastructure.Interfaces.CommercialFishing;

public interface ITransportLineService
{
    Task<IEnumerable<TransportLineResponseDTO>> GetAllAsync(BaseFilter<TransportLineFilter> filters);
    Task<TransportLineResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(TransportLineRequestDTO line);
    Task<bool> EditAsync(TransportLineRequestDTO line);
    Task<bool> DeleteAsync(int id);
}

