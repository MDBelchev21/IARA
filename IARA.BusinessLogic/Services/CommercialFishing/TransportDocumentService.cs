using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.CommercialFishing;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.BusinessLogic.Services.CommercialFishing;

public class TransportDocumentService : BaseService, ITransportDocumentService
{
    public TransportDocumentService(BaseServiceInjector injector) : base(injector)
    {
    }

    public async Task<IEnumerable<TransportDocumentResponseDTO>> GetAllAsync(BaseFilter<TransportDocumentFilter> filters)
    {
        IQueryable<TransportDocument> query = ApplyFilters(GetAllFromDatabase(), filters.Filters);

        if (!string.IsNullOrEmpty(filters.FreeTextSearch))
        {
            query = ApplyFreeTextSearch(query, filters.FreeTextSearch);
        }

        query = ApplyPagination(query, filters.Page, filters.PageSize);

        return await ApplyMapping(query).ToListAsync();
    }

    public async Task<TransportDocumentResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(d => d.DocumentId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(TransportDocumentRequestDTO document)
    {
        TransportDocument entity = new TransportDocument
        {
            DocumentNumber = document.DocumentNumber,
            TransportDate = document.TransportDate,
            OriginLocation = document.OriginLocation,
            DestinationLocation = document.DestinationLocation,
            VehicleRegistration = document.VehicleRegistration,
            DriverName = document.DriverName,
            ReceivedOn = document.ReceivedOn
        };

        Db.TransportDocuments.Add(entity);
        await Db.SaveChangesAsync();

        return entity.DocumentId;
    }

    public async Task<bool> EditAsync(TransportDocumentRequestDTO document)
    {
        if (!document.DocumentId.HasValue)
        {
            throw new ArgumentException("DocumentId is required for edit operation");
        }

        TransportDocument entity = await GetAllFromDatabase().Where(d => d.DocumentId == document.DocumentId.Value).SingleAsync();

        entity.DocumentNumber = document.DocumentNumber;
        entity.TransportDate = document.TransportDate;
        entity.OriginLocation = document.OriginLocation;
        entity.DestinationLocation = document.DestinationLocation;
        entity.VehicleRegistration = document.VehicleRegistration;
        entity.DriverName = document.DriverName;
        entity.ReceivedOn = document.ReceivedOn;

        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        TransportDocument entity = await GetAllFromDatabase().Where(d => d.DocumentId == id).SingleAsync();
        Db.TransportDocuments.Remove(entity);
        return await Db.SaveChangesAsync() > 0;
    }

    private IQueryable<TransportDocument> ApplyPagination(IQueryable<TransportDocument> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    private IQueryable<TransportDocument> ApplyFreeTextSearch(IQueryable<TransportDocument> query, string text)
    {
        return query.Where(x =>
            x.DocumentNumber.Contains(text) ||
            (x.OriginLocation != null && x.OriginLocation.Contains(text)) ||
            x.DestinationLocation.Contains(text) ||
            (x.VehicleRegistration != null && x.VehicleRegistration.Contains(text)) ||
            (x.DriverName != null && x.DriverName.Contains(text)));
    }

    private IQueryable<TransportDocumentResponseDTO> ApplyMapping(IQueryable<TransportDocument> query)
    {
        return query.Select(d => new TransportDocumentResponseDTO
        {
            DocumentId = d.DocumentId,
            DocumentNumber = d.DocumentNumber,
            TransportDate = d.TransportDate,
            OriginLocation = d.OriginLocation,
            DestinationLocation = d.DestinationLocation,
            VehicleRegistration = d.VehicleRegistration,
            DriverName = d.DriverName,
            ReceivedOn = d.ReceivedOn,
            TransportLinesCount = d.TransportLines.Count,
            InspectionsCount = d.Inspections.Count
        });
    }

    private IQueryable<TransportDocument> ApplyFilters(IQueryable<TransportDocument> query, TransportDocumentFilter? filters)
    {
        if (filters == null)
        {
            return query;
        }

        if (!string.IsNullOrEmpty(filters.DocumentNumber))
        {
            query = query.Where(d => d.DocumentNumber == filters.DocumentNumber);
        }

        if (!string.IsNullOrEmpty(filters.OriginLocation))
        {
            query = query.Where(d => d.OriginLocation == filters.OriginLocation);
        }

        if (!string.IsNullOrEmpty(filters.DestinationLocation))
        {
            query = query.Where(d => d.DestinationLocation == filters.DestinationLocation);
        }

        if (filters.TransportDateFrom.HasValue)
        {
            query = query.Where(d => d.TransportDate >= filters.TransportDateFrom.Value);
        }

        if (filters.TransportDateTo.HasValue)
        {
            query = query.Where(d => d.TransportDate <= filters.TransportDateTo.Value);
        }

        if (filters.IsReceived.HasValue)
        {
            if (filters.IsReceived.Value)
            {
                query = query.Where(d => d.ReceivedOn != null);
            }
            else
            {
                query = query.Where(d => d.ReceivedOn == null);
            }
        }

        return query;
    }

    private IQueryable<TransportDocument> GetAllFromDatabase()
    {
        return Db.TransportDocuments.AsQueryable();
    }
}

