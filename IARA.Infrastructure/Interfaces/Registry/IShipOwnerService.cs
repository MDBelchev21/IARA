using IARA.DomainModel;
using IARA.DomainModel.DTOs.Registry;
using IARA.DomainModel.Filters.Registry;

namespace IARA.Infrastructure.Interfaces.Registry;

public interface IShipOwnerService
{
    Task<IEnumerable<ShipOwnerResponseDTO>> GetAllAsync(BaseFilter<PersonFilter> filters);
    Task<ShipOwnerResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(ShipOwnerRequestDTO owner);
    Task<bool> EditAsync(ShipOwnerRequestDTO owner);
    Task<bool> DeleteAsync(int id);
}

