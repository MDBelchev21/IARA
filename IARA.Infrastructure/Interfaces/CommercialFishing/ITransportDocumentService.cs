using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;

namespace IARA.Infrastructure.Interfaces.CommercialFishing;

public interface ITransportDocumentService
{
    Task<IEnumerable<TransportDocumentResponseDTO>> GetAllAsync(BaseFilter<TransportDocumentFilter> filters);
    Task<TransportDocumentResponseDTO?> GetAsync(int id);
    Task<int> AddAsync(TransportDocumentRequestDTO document);
    Task<bool> EditAsync(TransportDocumentRequestDTO document);
    Task<bool> DeleteAsync(int id);
}

