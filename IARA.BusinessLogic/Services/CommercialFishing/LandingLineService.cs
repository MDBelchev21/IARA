using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.CommercialFishing;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.BusinessLogic.Services.CommercialFishing;

public class LandingLineService : BaseService, ILandingLineService
{
    public LandingLineService(BaseServiceInjector injector) : base(injector)
    {
    }

    public async Task<IEnumerable<LandingLineResponseDTO>> GetAllAsync(BaseFilter<LandingLineFilter> filters)
    {
        IQueryable<LandingLine> query = ApplyFilters(GetAllFromDatabase(), filters.Filters);

        if (!string.IsNullOrEmpty(filters.FreeTextSearch))
        {
            query = ApplyFreeTextSearch(query, filters.FreeTextSearch);
        }

        query = ApplyPagination(query, filters.Page, filters.PageSize);

        return await ApplyMapping(query).ToListAsync();
    }

    public async Task<LandingLineResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(l => l.LandingLineId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(LandingLineRequestDTO line)
    {
        LandingLine entity = new LandingLine
        {
            LandingId = line.LandingId,
            CatchId = line.CatchId,
            BatchNumber = line.BatchNumber,
            SpeciesName = line.SpeciesName,
            WeightKg = line.WeightKg
        };

        Db.LandingLines.Add(entity);
        await Db.SaveChangesAsync();

        return entity.LandingLineId;
    }

    public async Task<bool> EditAsync(LandingLineRequestDTO line)
    {
        if (!line.LandingLineId.HasValue)
        {
            throw new ArgumentException("LandingLineId is required for edit operation");
        }

        LandingLine entity = await GetAllFromDatabase().Where(l => l.LandingLineId == line.LandingLineId.Value).SingleAsync();

        entity.LandingId = line.LandingId;
        entity.CatchId = line.CatchId;
        entity.BatchNumber = line.BatchNumber;
        entity.SpeciesName = line.SpeciesName;
        entity.WeightKg = line.WeightKg;

        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        LandingLine entity = await GetAllFromDatabase().Where(l => l.LandingLineId == id).SingleAsync();
        Db.LandingLines.Remove(entity);
        return await Db.SaveChangesAsync() > 0;
    }

    private IQueryable<LandingLine> ApplyPagination(IQueryable<LandingLine> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    private IQueryable<LandingLine> ApplyFreeTextSearch(IQueryable<LandingLine> query, string text)
    {
        return query.Where(x =>
            x.BatchNumber.Contains(text) ||
            x.SpeciesName.Contains(text));
    }

    private IQueryable<LandingLineResponseDTO> ApplyMapping(IQueryable<LandingLine> query)
    {
        return query.Select(line => new LandingLineResponseDTO
        {
            LandingLineId = line.LandingLineId,
            LandingId = line.LandingId,
            CatchId = line.CatchId,
            BatchNumber = line.BatchNumber,
            SpeciesName = line.SpeciesName,
            WeightKg = line.WeightKg
        });
    }

    private IQueryable<LandingLine> ApplyFilters(IQueryable<LandingLine> query, LandingLineFilter? filters)
    {
        if (filters == null)
        {
            return query;
        }

        if (filters.LandingId.HasValue)
        {
            query = query.Where(l => l.LandingId == filters.LandingId.Value);
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

    private IQueryable<LandingLine> GetAllFromDatabase()
    {
        return Db.LandingLines.AsQueryable();
    }
}

