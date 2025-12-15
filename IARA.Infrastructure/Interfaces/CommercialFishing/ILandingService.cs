using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;

namespace IARA.Infrastructure.Interfaces.CommercialFishing;

public interface ILandingService
{
    Task<IEnumerable<LandingResponseDTO>> GetAllAsync(BaseFilter<LandingFilter> filters);
    Task<LandingResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(LandingRequestDTO landing);
    Task<bool> EditAsync(LandingRequestDTO landing);
    Task<bool> DeleteAsync(int id);
}
