using IARA.DomainModel;
using IARA.DomainModel.DTOs.Registry;
using IARA.DomainModel.Filters.Registry;

namespace IARA.Infrastructure.Interfaces.Registry;

public interface IQualificationService
{
    Task<IEnumerable<QualificationResponseDTO>> GetAllAsync(BaseFilter<PersonFilter> filters);
    Task<QualificationResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(QualificationRequestDTO entity);
    Task<bool> EditAsync(QualificationRequestDTO entity);
    Task<bool> DeleteAsync(int id);
}

