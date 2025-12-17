using IARA.DomainModel;
using IARA.DomainModel.DTOs.Registry;
using IARA.DomainModel.Filters.Registry;

namespace IARA.Infrastructure.Interfaces.Registry;

public interface IAdministratorService
{
    Task<IEnumerable<PersonResponseDTO>> GetAllAsync(BaseFilter<PersonFilter> filters);
    Task<PersonResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(PersonRequestDTO person);
    Task<bool> DeleteAsync(int id);
}
