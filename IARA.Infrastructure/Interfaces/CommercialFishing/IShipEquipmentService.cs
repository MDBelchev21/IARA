using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;

namespace IARA.Infrastructure.Interfaces.CommercialFishing;

public interface IShipEquipmentService
{
    Task<IEnumerable<ShipEquipmentResponseDTO>> GetAllAsync(BaseFilter<ShipEquipmentFilter> filters);
    Task<ShipEquipmentResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(ShipEquipmentRequestDTO equipment);
    Task<bool> EditAsync(ShipEquipmentRequestDTO equipment);
    Task<bool> DeleteAsync(int id);
}
