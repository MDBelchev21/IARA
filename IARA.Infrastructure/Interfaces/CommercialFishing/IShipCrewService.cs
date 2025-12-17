using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;

namespace IARA.Infrastructure.Interfaces.CommercialFishing;

public interface IShipCrewService
{
    Task<IEnumerable<ShipCrewResponseDTO>> GetAllAsync(BaseFilter<ShipCrewFilter> filters);
    Task<ShipCrewResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(ShipCrewRequestDTO entity);
    Task<bool> EditAsync(ShipCrewRequestDTO entity);
    Task<bool> DeleteAsync(int id);
}

