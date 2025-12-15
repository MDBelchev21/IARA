using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;

namespace IARA.Infrastructure.Interfaces.CommercialFishing;

public interface IFishingTripService
{
    Task<IEnumerable<FishingTripResponseDTO>> GetAllAsync(BaseFilter<FishingTripFilter> filters);
    Task<FishingTripResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(FishingTripRequestDTO trip);
    Task<bool> EditAsync(FishingTripRequestDTO trip);
    Task<bool> DeleteAsync(int id);
}

