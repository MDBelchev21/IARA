using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.CommercialFishing;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.BusinessLogic.Services.CommercialFishing;

public class TransportLineService : BaseService, ITransportLineService
{
    public TransportLineService(BaseServiceInjector injector) : base(injector)
    {
    }

    public async Task<IEnumerable<TransportLineResponseDTO>> GetAllAsync(BaseFilter<TransportLineFilter> filters)
    {
        IQueryable<TransportLine> query = ApplyFilters(GetAllFromDatabase(), filters.Filters);

        if (!string.IsNullOrEmpty(filters.FreeTextSearch))
        {
            query = ApplyFreeTextSearch(query, filters.FreeTextSearch);
        }

        query = ApplyPagination(query, filters.Page, filters.PageSize);

        return await ApplyMapping(query).ToListAsync();
    }

    public async Task<TransportLineResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(l => l.TransportLineId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(TransportLineRequestDTO line)
    {
        TransportLine entity = new TransportLine
        {
            DocumentId = line.DocumentId,
            BatchNumber = line.BatchNumber,
            SpeciesName = line.SpeciesName,
            WeightKg = line.WeightKg
        };

        Db.TransportLines.Add(entity);
        await Db.SaveChangesAsync();

        return entity.TransportLineId;
    }

    public async Task<bool> EditAsync(TransportLineRequestDTO line)
    {
        if (!line.TransportLineId.HasValue)
        {
            throw new ArgumentException("TransportLineId is required for edit operation");
        }

        TransportLine entity = await GetAllFromDatabase().Where(l => l.TransportLineId == line.TransportLineId.Value).SingleAsync();

        entity.DocumentId = line.DocumentId;
        entity.BatchNumber = line.BatchNumber;
        entity.SpeciesName = line.SpeciesName;
        entity.WeightKg = line.WeightKg;

        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        TransportLine entity = await GetAllFromDatabase().Where(l => l.TransportLineId == id).SingleAsync();
        Db.TransportLines.Remove(entity);
        return await Db.SaveChangesAsync() > 0;
    }

    private IQueryable<TransportLine> ApplyPagination(IQueryable<TransportLine> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    private IQueryable<TransportLine> ApplyFreeTextSearch(IQueryable<TransportLine> query, string text)
    {
        return query.Where(x =>
            x.BatchNumber.Contains(text) ||
            x.SpeciesName.Contains(text));
    }

    private IQueryable<TransportLineResponseDTO> ApplyMapping(IQueryable<TransportLine> query)
    {
        return query.Select(line => new TransportLineResponseDTO
        {
            TransportLineId = line.TransportLineId,
            DocumentId = line.DocumentId,
            BatchNumber = line.BatchNumber,
            SpeciesName = line.SpeciesName,
            WeightKg = line.WeightKg
        });
    }

    private IQueryable<TransportLine> ApplyFilters(IQueryable<TransportLine> query, TransportLineFilter? filters)
    {
        if (filters == null)
        {
            return query;
        }

        if (filters.DocumentId.HasValue)
        {
            query = query.Where(l => l.DocumentId == filters.DocumentId.Value);
        }

        if (!string.IsNullOrEmpty(filters.BatchNumber))
        {
            query = query.Where(l => l.BatchNumber == filters.BatchNumber);
        }

        if (!string.IsNullOrEmpty(filters.SpeciesName))
        {
            query = query.Where(l => l.SpeciesName == filters.SpeciesName);
        }

        return query;
    }

    private IQueryable<TransportLine> GetAllFromDatabase()
    {
        return Db.TransportLines.AsQueryable();
    }
}

