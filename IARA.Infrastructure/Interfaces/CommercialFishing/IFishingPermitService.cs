using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;

namespace IARA.Infrastructure.Interfaces.CommercialFishing;

public interface IFishingPermitService
{
    Task<IEnumerable<FishingPermitResponseDTO>> GetAllAsync(BaseFilter<FishingPermitFilter> filters);
    Task<FishingPermitResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(FishingPermitRequestDTO permit);
    Task<bool> EditAsync(FishingPermitRequestDTO permit);
    Task<bool> DeleteAsync(int id);
    Task<bool> RevokePermitAsync(int id);
    Task<bool> IsPermitValidAsync(int id);
}
