using IARA.DomainModel;
using IARA.DomainModel.DTOs.Registry;
using IARA.DomainModel.Filters.Registry;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.Registry;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.BusinessLogic.Services.Registry;

public class ShipOwnerService : BaseService, IShipOwnerService
{
    public ShipOwnerService(BaseServiceInjector injector) : base(injector)
    {
    }

    public async Task<IEnumerable<ShipOwnerResponseDTO>> GetAllAsync(BaseFilter<PersonFilter> filters)
    {
        IQueryable<ShipOwnerResponseDTO> query;

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

    public async Task<ShipOwnerResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(o => o.ShipOwnerId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(ShipOwnerRequestDTO owner)
    {
        ShipOwner entity = new ShipOwner()
        {
            ShipId = owner.ShipId,
            PersonId = owner.PersonId,
            LegalEntityId = owner.LegalEntityId,
            OwnershipShare = owner.OwnershipShare,
            ValidFrom = owner.ValidFrom,
            ValidTo = owner.ValidTo,
            IsActive = owner.IsActive
        };

        Db.ShipOwners.Add(entity);
        await Db.SaveChangesAsync();

        return entity.ShipOwnerId;
    }

    public async Task<bool> EditAsync(ShipOwnerRequestDTO owner)
    {
        if (!owner.ShipOwnerId.HasValue)
        {
            throw new ArgumentException("ShipOwnerId is required for edit operation");
        }

        ShipOwner entity = await GetAllFromDatabase()
            .Where(o => o.ShipOwnerId == owner.ShipOwnerId.Value)
            .SingleAsync();

        entity.ShipId = owner.ShipId;
        entity.PersonId = owner.PersonId;
        entity.LegalEntityId = owner.LegalEntityId;
        entity.OwnershipShare = owner.OwnershipShare;
        entity.ValidFrom = owner.ValidFrom;
        entity.ValidTo = owner.ValidTo;
        entity.IsActive = owner.IsActive;

        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var owner = await GetAllFromDatabase().Where(o => o.ShipOwnerId == id).SingleAsync();
        owner.IsActive = false;
        owner.ValidTo = DateOnly.FromDateTime(DateTime.UtcNow);
        return await Db.SaveChangesAsync() > 0;
    }

    private IQueryable<ShipOwner> ApplyPagination(IQueryable<ShipOwner> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    private IQueryable<ShipOwner> ApplyFreeTextSearch(IQueryable<ShipOwner> query, string text)
    {
        return query.Where(x =>
            x.Ship.Name.Contains(text) ||
            (x.Person != null && ((x.Person.FirstName + " " + (x.Person.MiddleName ?? "") + " " + x.Person.LastName).Contains(text))) ||
            (x.LegalEntity != null && x.LegalEntity.Name.Contains(text)));
    }

    private IQueryable<ShipOwnerResponseDTO> ApplyMapping(IQueryable<ShipOwner> query)
    {
        return query.Select(owner => new ShipOwnerResponseDTO()
        {
            ShipOwnerId = owner.ShipOwnerId,
            ShipId = owner.ShipId,
            PersonId = owner.PersonId,
            LegalEntityId = owner.LegalEntityId,
            OwnershipShare = owner.OwnershipShare,
            ValidFrom = owner.ValidFrom,
            ValidTo = owner.ValidTo,
            IsActive = owner.IsActive,
            ShipName = owner.Ship.Name,
            PersonFullName = owner.Person != null ? owner.Person.FirstName + " " + (owner.Person.MiddleName != null ? owner.Person.MiddleName + " " : "") + owner.Person.LastName : null,
            LegalEntityName = owner.LegalEntity != null ? owner.LegalEntity.Name : null
        });
    }

    private IQueryable<ShipOwner> ApplyFilters(IQueryable<ShipOwner> query, PersonFilter? filters)
    {
        query = query.Where(o => o.IsActive);

        if (filters == null)
        {
            return query;
        }

        if (!string.IsNullOrEmpty(filters.FirstName) || !string.IsNullOrEmpty(filters.LastName))
        {
            query = query.Where(o => o.Person != null &&
                                     (string.IsNullOrEmpty(filters.FirstName) || o.Person.FirstName == filters.FirstName) &&
                                     (string.IsNullOrEmpty(filters.LastName) || o.Person.LastName == filters.LastName));
        }

        if (!string.IsNullOrEmpty(filters.EGN))
        {
            query = query.Where(o => o.Person != null && o.Person.EGN == filters.EGN);
        }

        if (!string.IsNullOrEmpty(filters.Email))
        {
            query = query.Where(o => o.Person != null && o.Person.Email == filters.Email);
        }

        if (!string.IsNullOrEmpty(filters.Phone))
        {
            query = query.Where(o => o.Person != null && o.Person.Phone == filters.Phone);
        }

        return query;
    }

    private IQueryable<ShipOwner> GetAllFromDatabase()
    {
        return Db.ShipOwners
            .Include(o => o.Ship)
            .Include(o => o.Person)
            .Include(o => o.LegalEntity)
            .AsQueryable();
    }
}

