using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.CommercialFishing;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.BusinessLogic.Services.CommercialFishing;

public class FishingPermitService : BaseService, IFishingPermitService
{
    public FishingPermitService(BaseServiceInjector injector) : base(injector)
    {
    }

    public async Task<IEnumerable<FishingPermitResponseDTO>> GetAllAsync(BaseFilter<FishingPermitFilter> filters)
    {
        IQueryable<FishingPermitResponseDTO> query;
        
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

    public async Task<FishingPermitResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(p => p.PermitId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(FishingPermitRequestDTO permit)
    {
        FishingPermit entity = new FishingPermit()
        {
            PermitNumber = GeneratePermitNumber(),
            ShipId = permit.ShipId,
            IssuedOn = DateOnly.FromDateTime(DateTime.Now),
            ValidFrom = DateOnly.FromDateTime(permit.ValidFrom),
            ValidUntil = DateOnly.FromDateTime(permit.ValidUntil),
            IsRevoked = false
        };

        Db.FishingPermits.Add(entity);
        await Db.SaveChangesAsync();

        return entity.PermitId;
    }

    public async Task<bool> EditAsync(FishingPermitRequestDTO permit)
    {
        if (!permit.PermitId.HasValue)
        {
            throw new ArgumentException("PermitId is required for edit operation");
        }

        FishingPermit entity = await GetAllFromDatabase()
            .Where(p => p.PermitId == permit.PermitId.Value)
            .SingleAsync();

        entity.ShipId = permit.ShipId;
        entity.ValidFrom = DateOnly.FromDateTime(permit.ValidFrom);
        entity.ValidUntil = DateOnly.FromDateTime(permit.ValidUntil);

        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetAllFromDatabase().Where(p => p.PermitId == id).SingleAsync();
        Db.FishingPermits.Remove(entity);
        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> RevokePermitAsync(int id)
    {
        var permit = await GetAllFromDatabase().Where(p => p.PermitId == id).SingleAsync();
        permit.IsRevoked = true;
        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> IsPermitValidAsync(int id)
    {
        var permit = await GetAllFromDatabase().Where(p => p.PermitId == id).SingleOrDefaultAsync();
        if (permit == null)
        {
            return false;
        }
        
        var today = DateOnly.FromDateTime(DateTime.Today);
        return !permit.IsRevoked && permit.ValidFrom <= today && permit.ValidUntil >= today;
    }

    private IQueryable<FishingPermit> ApplyPagination(IQueryable<FishingPermit> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    private IQueryable<FishingPermit> ApplyFreeTextSearch(IQueryable<FishingPermit> query, string text)
    {
        return query.Where(x =>
            x.PermitNumber.Contains(text) ||
            (x.Ship.Name != null && x.Ship.Name.Contains(text)) ||
            x.Ship.ExternalMarking.Contains(text));
    }

    private IQueryable<FishingPermitResponseDTO> ApplyMapping(IQueryable<FishingPermit> query)
    {
        return (from permit in query
            join ship in Db.Ships on permit.ShipId equals ship.ShipId
            join shipOwner in Db.ShipOwners on ship.ShipId equals shipOwner.ShipId
            join owner in Db.Persons on shipOwner.PersonId equals owner.PersonId
            where shipOwner.IsActive
            select new FishingPermitResponseDTO()
            {
                PermitId = permit.PermitId,
                PermitNumber = permit.PermitNumber,
                ShipName = ship.Name ?? ship.ExternalMarking,
                ShipMarking = ship.ExternalMarking,
                OwnerName = owner.FirstName + " " + owner.LastName,
                IssuedOn = permit.IssuedOn.ToDateTime(TimeOnly.MinValue),
                ValidFrom = permit.ValidFrom.ToDateTime(TimeOnly.MinValue),
                ValidUntil = permit.ValidUntil.ToDateTime(TimeOnly.MinValue),
                IsActive = !permit.IsRevoked,
                EquipmentCount = permit.PermitEquipments.Count
            });
    }

    private IQueryable<FishingPermit> ApplyFilters(IQueryable<FishingPermit> query, FishingPermitFilter? filters)
    {
        if (filters == null)
        {
            return query;
        }

        if (filters.ShipId.HasValue)
        {
            query = query.Where(p => p.ShipId == filters.ShipId.Value);
        }

        if (filters.OwnerId.HasValue)
        {
            query = query.Where(p => p.Ship.ShipOwners.Any(so => so.PersonId == filters.OwnerId.Value && so.IsActive));
        }

        if (filters.IssuedOnFrom.HasValue)
        {
            var dateOnly = DateOnly.FromDateTime(filters.IssuedOnFrom.Value);
            query = query.Where(p => p.IssuedOn >= dateOnly);
        }

        if (filters.IssuedOnTo.HasValue)
        {
            var dateOnly = DateOnly.FromDateTime(filters.IssuedOnTo.Value);
            query = query.Where(p => p.IssuedOn <= dateOnly);
        }

        if (filters.ValidFrom.HasValue)
        {
            var dateOnly = DateOnly.FromDateTime(filters.ValidFrom.Value);
            query = query.Where(p => p.ValidFrom >= dateOnly);
        }

        if (filters.ValidTo.HasValue)
        {
            var dateOnly = DateOnly.FromDateTime(filters.ValidTo.Value);
            query = query.Where(p => p.ValidUntil <= dateOnly);
        }

        if (filters.IsActive.HasValue)
        {
            var isRevoked = !filters.IsActive.Value;
            query = query.Where(p => p.IsRevoked == isRevoked);
        }

        return query;
    }

    private IQueryable<FishingPermit> GetAllFromDatabase()
    {
        return Db.FishingPermits.AsQueryable();
    }

    private string GeneratePermitNumber()
    {
        return $"FP-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }
}
