using IARA.DomainModel;
using IARA.DomainModel.DTOs.RecreationalFishing;
using IARA.DomainModel.Filters.RecreationalFishing;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.RecreationalFishing;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.BusinessLogic.Services.RecreationalFishing;

public class RecreationalFishermanService : BaseService, IRecreationalFishermanService
{
    public RecreationalFishermanService(BaseServiceInjector injector) : base(injector)
    {
    }

    public async Task<IEnumerable<RecreationalFishermanResponseDTO>> GetAllAsync(BaseFilter<RecreationalFishermanFilter> filters)
    {
        IQueryable<RecreationalFishermanResponseDTO> query;
        
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

    public async Task<RecreationalFishermanResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(f => f.RecFishermanId == id)).FirstOrDefaultAsync();
    }

    public async Task<int?> GetByPersonIdAsync(int personId)
    {
        var fisherman = await Db.RecreationalFishermen
            .Where(f => f.PersonId == personId)
            .FirstOrDefaultAsync();
        
        return fisherman?.RecFishermanId;
    }

    public async Task<int> AddAsync(RecreationalFishermanRequestDTO fisherman)
    {
        RecreationalFisherman entity = new RecreationalFisherman()
        {
            PersonId = fisherman.PersonId
        };

        Db.RecreationalFishermen.Add(entity);
        await Db.SaveChangesAsync();

        return entity.RecFishermanId;
    }

    public async Task<bool> EditAsync(RecreationalFishermanRequestDTO fisherman)
    {
        if (!fisherman.RecFishermanId.HasValue)
        {
            throw new ArgumentException("RecFishermanId is required for edit operation");
        }

        RecreationalFisherman entity = await GetAllFromDatabase()
            .Where(f => f.RecFishermanId == fisherman.RecFishermanId.Value)
            .SingleAsync();

        entity.PersonId = fisherman.PersonId;

        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetAllFromDatabase().Where(f => f.RecFishermanId == id).SingleAsync();
        Db.RecreationalFishermen.Remove(entity);
        return await Db.SaveChangesAsync() > 0;
    }

    private IQueryable<RecreationalFisherman> ApplyPagination(IQueryable<RecreationalFisherman> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    private IQueryable<RecreationalFisherman> ApplyFreeTextSearch(IQueryable<RecreationalFisherman> query, string text)
    {
        return query.Where(x =>
            x.Person.FirstName.Contains(text) ||
            x.Person.LastName.Contains(text) ||
            (x.Person.Email != null && x.Person.Email.Contains(text)));
    }

    private IQueryable<RecreationalFishermanResponseDTO> ApplyMapping(IQueryable<RecreationalFisherman> query)
    {
        return (from fisherman in query
            join person in Db.Persons on fisherman.PersonId equals person.PersonId
            select new RecreationalFishermanResponseDTO()
            {
                RecFishermanId = fisherman.RecFishermanId,
                FullName = person.FirstName + " " + person.LastName,
                EGN = person.EGN,
                Email = person.Email,
                Phone = person.Phone,
                IsDisabled = fisherman.IsDisabled,
                TELKDecisionNumber = fisherman.TELKDecisionNumber,
                ActiveTicketsCount = fisherman.RecreationalTickets.Count(t => t.IsActive)
            });
    }

    private IQueryable<RecreationalFisherman> ApplyFilters(IQueryable<RecreationalFisherman> query, RecreationalFishermanFilter? filters)
    {
        if (filters == null)
        {
            return query;
        }

        if (!string.IsNullOrEmpty(filters.FirstName))
        {
            query = query.Where(f => f.Person.FirstName == filters.FirstName);
        }

        if (!string.IsNullOrEmpty(filters.LastName))
        {
            query = query.Where(f => f.Person.LastName == filters.LastName);
        }

        if (!string.IsNullOrEmpty(filters.EGN))
        {
            query = query.Where(f => f.Person.EGN == filters.EGN);
        }

        if (!string.IsNullOrEmpty(filters.Email))
        {
            query = query.Where(f => f.Person.Email == filters.Email);
        }

        return query;
    }

    private IQueryable<RecreationalFisherman> GetAllFromDatabase()
    {
        return Db.RecreationalFishermen.AsQueryable();
    }
}

