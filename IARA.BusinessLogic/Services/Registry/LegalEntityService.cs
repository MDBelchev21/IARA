using IARA.DomainModel;
using IARA.DomainModel.DTOs.Registry;
using IARA.DomainModel.Filters.Registry;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.Registry;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.BusinessLogic.Services.Registry;

public class LegalEntityService : BaseService, ILegalEntityService
{
    public LegalEntityService(BaseServiceInjector injector) : base(injector)
    {
    }

    public async Task<IEnumerable<LegalEntityResponseDTO>> GetAllAsync(BaseFilter<PersonFilter> filters)
    {
        IQueryable<LegalEntityResponseDTO> query;

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

    public async Task<LegalEntityResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(le => le.LegalEntityId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(LegalEntityRequestDTO entity)
    {
        LegalEntity dbEntity = new LegalEntity()
        {
            Name = entity.Name,
            EIK = entity.EIK,
            Address = entity.Address,
            Email = entity.Email,
            Phone = entity.Phone,
            IsDeleted = false
        };

        Db.LegalEntities.Add(dbEntity);
        await Db.SaveChangesAsync();

        return dbEntity.LegalEntityId;
    }

    public async Task<bool> EditAsync(LegalEntityRequestDTO entity)
    {
        if (!entity.LegalEntityId.HasValue)
        {
            throw new ArgumentException("LegalEntityId is required for edit operation");
        }

        LegalEntity dbEntity = await GetAllFromDatabase()
            .Where(le => le.LegalEntityId == entity.LegalEntityId.Value)
            .SingleAsync();

        dbEntity.Name = entity.Name;
        dbEntity.EIK = entity.EIK;
        dbEntity.Address = entity.Address;
        dbEntity.Email = entity.Email;
        dbEntity.Phone = entity.Phone;

        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetAllFromDatabase().Where(le => le.LegalEntityId == id).SingleAsync();
        entity.IsDeleted = true;
        return await Db.SaveChangesAsync() > 0;
    }

    private IQueryable<LegalEntity> ApplyPagination(IQueryable<LegalEntity> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    private IQueryable<LegalEntity> ApplyFreeTextSearch(IQueryable<LegalEntity> query, string text)
    {
        return query.Where(x =>
            x.Name.Contains(text) ||
            x.EIK.Contains(text) ||
            (x.Email != null && x.Email.Contains(text)) ||
            (x.Address != null && x.Address.Contains(text)));
    }

    private IQueryable<LegalEntityResponseDTO> ApplyMapping(IQueryable<LegalEntity> query)
    {
        return query.Select(le => new LegalEntityResponseDTO()
        {
            LegalEntityId = le.LegalEntityId,
            Name = le.Name,
            EIK = le.EIK,
            Address = le.Address,
            Email = le.Email,
            Phone = le.Phone,
            IsDeleted = le.IsDeleted
        });
    }

    private IQueryable<LegalEntity> ApplyFilters(IQueryable<LegalEntity> query, PersonFilter? filters)
    {
        query = query.Where(le => !le.IsDeleted);

        if (filters == null)
        {
            return query;
        }

        if (!string.IsNullOrEmpty(filters.FirstName) || !string.IsNullOrEmpty(filters.LastName))
        {
            // No direct person link; ignore name filters
        }

        if (!string.IsNullOrEmpty(filters.EGN))
        {
            // No EGN on legal entity; ignore
        }

        if (!string.IsNullOrEmpty(filters.Email))
        {
            query = query.Where(le => le.Email == filters.Email);
        }

        if (!string.IsNullOrEmpty(filters.Phone))
        {
            query = query.Where(le => le.Phone == filters.Phone);
        }

        return query;
    }

    private IQueryable<LegalEntity> GetAllFromDatabase()
    {
        return Db.LegalEntities.AsQueryable();
    }
}

