using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;

namespace IARA.Infrastructure.Interfaces.CommercialFishing;

public interface ILandingLineService
{
    Task<IEnumerable<LandingLineResponseDTO>> GetAllAsync(BaseFilter<LandingLineFilter> filters);
    Task<LandingLineResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(LandingLineRequestDTO line);
    Task<bool> EditAsync(LandingLineRequestDTO line);
    Task<bool> DeleteAsync(int id);
}

