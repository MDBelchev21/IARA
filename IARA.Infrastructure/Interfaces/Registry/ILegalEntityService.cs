using IARA.DomainModel;
using IARA.DomainModel.DTOs.Registry;
using IARA.DomainModel.Filters.Registry;

namespace IARA.Infrastructure.Interfaces.Registry;

public interface ILegalEntityService
{
    Task<IEnumerable<LegalEntityResponseDTO>> GetAllAsync(BaseFilter<PersonFilter> filters);
    Task<LegalEntityResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(LegalEntityRequestDTO entity);
    Task<bool> EditAsync(LegalEntityRequestDTO entity);
    Task<bool> DeleteAsync(int id);
}

