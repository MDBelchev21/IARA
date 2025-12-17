using IARA.DomainModel;
using IARA.DomainModel.DTOs.RecreationalFishing;
using IARA.DomainModel.Filters.RecreationalFishing;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.RecreationalFishing;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IARA.BusinessLogic.Services.RecreationalFishing;

public class RecreationalCatchService : BaseService, IRecreationalCatchService
{
    public RecreationalCatchService(BaseServiceInjector injector) : base(injector) { }

    public async Task<IEnumerable<RecreationalCatchResponseDTO>> GetAllAsync(BaseFilter<RecreationalCatchFilter> filters)
    {
        IQueryable<RecreationalCatchResponseDTO> query;
        if (string.IsNullOrEmpty(filters.FreeTextSearch))
        {
            query = ApplyMapping(ApplyPagination(ApplyFilters(GetAllFromDatabase(), filters.Filters), filters.Page, filters.PageSize));
        }
        else
        {
            query = ApplyMapping(ApplyPagination(ApplyFreeTextSearch(GetAllFromDatabase(), filters.FreeTextSearch), filters.Page, filters.PageSize));
        }
        return await query.ToListAsync();
    }

    public async Task<RecreationalCatchResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(Queryable.Where(GetAllFromDatabase(), c => c.RecCatchId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(RecreationalCatchRequestDTO catchDto)
    {
        RecreationalCatch entity = new RecreationalCatch()
        {
            TicketId = catchDto.TicketId,
            CatchDate = catchDto.CatchDate,
            SpeciesName = catchDto.SpeciesName,
            WeightKg = catchDto.WeightKg,
            Location = catchDto.Location,
            Quantity = catchDto.Quantity,
            RegisteredVia = catchDto.RegisteredVia
        };
        Db.RecreationalCatches.Add(entity);
        await Db.SaveChangesAsync();
        return entity.RecCatchId;
    }

    public async Task<bool> EditAsync(RecreationalCatchRequestDTO catchDto)
    {
        if (!catchDto.RecCatchId.HasValue) throw new ArgumentException("RecCatchId is required");
        var entity = await Queryable.Where(GetAllFromDatabase(), c => c.RecCatchId == catchDto.RecCatchId.Value).SingleAsync();
        entity.CatchDate = catchDto.CatchDate;
        entity.SpeciesName = catchDto.SpeciesName;
        entity.WeightKg = catchDto.WeightKg;
        entity.Location = catchDto.Location;
        entity.Quantity = catchDto.Quantity;
        entity.RegisteredVia = catchDto.RegisteredVia;
        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await Queryable.Where(GetAllFromDatabase(), c => c.RecCatchId == id).SingleAsync();
        Db.RecreationalCatches.Remove(entity);
        return await Db.SaveChangesAsync() > 0;
    }

    private IQueryable<RecreationalCatch> ApplyPagination(IQueryable<RecreationalCatch> query, int page, int pageSize) => Queryable.Skip(query, (page - 1) * pageSize).Take(pageSize);
    
    private IQueryable<RecreationalCatch> ApplyFreeTextSearch(IQueryable<RecreationalCatch> query, string text) => Queryable.Where(query, x => x.SpeciesName.Contains(text) || (x.Location != null && x.Location.Contains(text)));
    
    private IQueryable<RecreationalCatchResponseDTO> ApplyMapping(IQueryable<RecreationalCatch> query) => Queryable.Select(query, c => new RecreationalCatchResponseDTO 
    { 
        RecCatchId = c.RecCatchId, 
        TicketId = c.TicketId, 
        CatchDate = c.CatchDate, 
        SpeciesName = c.SpeciesName, 
        WeightKg = c.WeightKg, 
        Location = c.Location,
        Quantity = c.Quantity,
        RegisteredVia = c.RegisteredVia
    });
    
    private IQueryable<RecreationalCatch> ApplyFilters(IQueryable<RecreationalCatch> query, RecreationalCatchFilter? filters)
    {
        if (filters == null) return query;
        if (filters.TicketId.HasValue) query = Queryable.Where(query, c => c.TicketId == filters.TicketId.Value);
        if (filters.FromDate.HasValue) query = Queryable.Where(query, c => c.CatchDate >= filters.FromDate.Value);
        if (filters.ToDate.HasValue) query = Queryable.Where(query, c => c.CatchDate <= filters.ToDate.Value);
        if (!string.IsNullOrEmpty(filters.SpeciesName)) query = Queryable.Where(query, c => c.SpeciesName == filters.SpeciesName);
        return query;
    }
    
    private IQueryable<RecreationalCatch> GetAllFromDatabase() => Db.RecreationalCatches.AsQueryable();
}
