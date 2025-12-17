using IARA.DomainModel;
using IARA.DomainModel.DTOs.RecreationalFishing;
using IARA.DomainModel.Filters.RecreationalFishing;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.RecreationalFishing;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IARA.BusinessLogic.Services.RecreationalFishing;

public class RecreationalTicketTypeService : BaseService, IRecreationalTicketTypeService
{
    public RecreationalTicketTypeService(BaseServiceInjector injector) : base(injector) { }

    public async Task<IEnumerable<RecreationalTicketTypeResponseDTO>> GetAllAsync(BaseFilter<RecreationalTicketTypeFilter> filters)
    {
        IQueryable<RecreationalTicketTypeResponseDTO> query;
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

    public async Task<RecreationalTicketTypeResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(t => t.TicketTypeId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(RecreationalTicketTypeRequestDTO type)
    {
        RecreationalTicketType entity = new RecreationalTicketType()
        {
            TypeName = type.Name,
            ValidityDays = type.ValidDays,
            PriceAdult = type.Price,
            PriceUnder14 = type.Price, // Simplification: if you need different prices, extend the DTO accordingly
            PricePensioner = type.Price,
            PriceDisabled = 0m
        };
        Db.RecreationalTicketTypes.Add(entity);
        await Db.SaveChangesAsync();
        return entity.TicketTypeId;
    }

    public async Task<bool> EditAsync(RecreationalTicketTypeRequestDTO type)
    {
        if (!type.TicketTypeId.HasValue) throw new ArgumentException("TicketTypeId is required");
        var entity = await GetAllFromDatabase().Where(t => t.TicketTypeId == type.TicketTypeId.Value).SingleAsync();
        entity.TypeName = type.Name;
        entity.ValidityDays = type.ValidDays;
        entity.PriceAdult = type.Price;
        entity.PriceUnder14 = type.Price;
        entity.PricePensioner = type.Price;
        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetAllFromDatabase().Where(t => t.TicketTypeId == id).SingleAsync();
        Db.RecreationalTicketTypes.Remove(entity);
        return await Db.SaveChangesAsync() > 0;
    }

    private IQueryable<RecreationalTicketType> ApplyPagination(IQueryable<RecreationalTicketType> query, int page, int pageSize) => Queryable.Skip(query, (page - 1) * pageSize).Take(pageSize);

    private IQueryable<RecreationalTicketType> ApplyFreeTextSearch(IQueryable<RecreationalTicketType> query, string text) => Queryable.Where(query, x => x.TypeName.Contains(text));

    private IQueryable<RecreationalTicketTypeResponseDTO> ApplyMapping(IQueryable<RecreationalTicketType> query) => Queryable.Select(query, t => new RecreationalTicketTypeResponseDTO { TicketTypeId = t.TicketTypeId, Name = t.TypeName, Price = t.PriceAdult, ValidDays = t.ValidityDays });

    private IQueryable<RecreationalTicketType> ApplyFilters(IQueryable<RecreationalTicketType> query, RecreationalTicketTypeFilter? filters)
    {
        if (filters == null) return query;
        if (!string.IsNullOrEmpty(filters.Name)) query = Queryable.Where(query, t => t.TypeName == filters.Name);
        if (filters.ValidDays.HasValue) query = Queryable.Where(query, t => t.ValidityDays == filters.ValidDays.Value);
        if (filters.MinPrice.HasValue) query = Queryable.Where(query, t => t.PriceAdult >= filters.MinPrice.Value);
        if (filters.MaxPrice.HasValue) query = Queryable.Where(query, t => t.PriceAdult <= filters.MaxPrice.Value);
        return query;
    }

    private IQueryable<RecreationalTicketType> GetAllFromDatabase() => Db.RecreationalTicketTypes.AsQueryable();
}
