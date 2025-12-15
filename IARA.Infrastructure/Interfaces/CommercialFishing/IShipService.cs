using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;

namespace IARA.Infrastructure.Interfaces.CommercialFishing;

public interface IShipService
{
    Task<IEnumerable<ShipResponseDTO>> GetAllAsync(BaseFilter<ShipFilter> filters);
    Task<ShipResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(ShipRequestDTO ship);
    Task<bool> EditAsync(ShipRequestDTO ship);
    Task<bool> DeleteAsync(int id);
}

