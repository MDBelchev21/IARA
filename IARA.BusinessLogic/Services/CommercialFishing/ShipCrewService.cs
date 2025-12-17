using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.CommercialFishing;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IARA.BusinessLogic.Services.CommercialFishing;

public class ShipCrewService : BaseService, IShipCrewService
{
    public ShipCrewService(BaseServiceInjector injector) : base(injector) { }

    public async Task<IEnumerable<ShipCrewResponseDTO>> GetAllAsync(BaseFilter<ShipCrewFilter> filters)
    {
        IQueryable<ShipCrewResponseDTO> query;
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

    public async Task<ShipCrewResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(sc => sc.ShipCrewId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(ShipCrewRequestDTO entity)
    {
        ShipCrew dbEntity = new ShipCrew()
        {
            ShipId = entity.ShipId,
            PersonId = entity.PersonId,
            Position = entity.Position,
            IsCaptain = entity.IsCaptain,
            QualificationId = entity.QualificationId,
            AssignedOn = entity.AssignedOn,
            RelievedOn = entity.RelievedOn,
            IsActive = entity.IsActive
        };
        Db.ShipCrews.Add(dbEntity);
        await Db.SaveChangesAsync();
        return dbEntity.ShipCrewId;
    }

    public async Task<bool> EditAsync(ShipCrewRequestDTO entity)
    {
        if (!entity.ShipCrewId.HasValue) throw new ArgumentException("ShipCrewId is required");
        var dbEntity = await GetAllFromDatabase().Where(sc => sc.ShipCrewId == entity.ShipCrewId.Value).SingleAsync();
        dbEntity.Position = entity.Position;
        dbEntity.IsCaptain = entity.IsCaptain;
        dbEntity.QualificationId = entity.QualificationId;
        dbEntity.AssignedOn = entity.AssignedOn;
        dbEntity.RelievedOn = entity.RelievedOn;
        dbEntity.IsActive = entity.IsActive;
        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var dbEntity = await GetAllFromDatabase().Where(sc => sc.ShipCrewId == id).SingleAsync();
        Db.ShipCrews.Remove(dbEntity);
        return await Db.SaveChangesAsync() > 0;
    }

    private IQueryable<ShipCrew> ApplyPagination(IQueryable<ShipCrew> query, int page, int pageSize) => Queryable.Skip(query, (page - 1) * pageSize).Take(pageSize);

    private IQueryable<ShipCrew> ApplyFreeTextSearch(IQueryable<ShipCrew> query, string text)
    {
        return Queryable.Where(query, x =>
            x.Position.Contains(text) ||
            x.Ship.Name.Contains(text) ||
            ((x.Person.FirstName + " " + (x.Person.MiddleName ?? "") + " " + x.Person.LastName).Contains(text))
        );
    }

    private IQueryable<ShipCrewResponseDTO> ApplyMapping(IQueryable<ShipCrew> query)
    {
        return Queryable.Select(query, sc => new ShipCrewResponseDTO()
        {
            ShipCrewId = sc.ShipCrewId,
            ShipId = sc.ShipId,
            PersonId = sc.PersonId,
            Position = sc.Position,
            IsCaptain = sc.IsCaptain,
            QualificationId = sc.QualificationId,
            AssignedOn = sc.AssignedOn,
            RelievedOn = sc.RelievedOn,
            IsActive = sc.IsActive,
            ShipName = sc.Ship.Name,
            PersonFullName = sc.Person.FirstName + " " + (sc.Person.MiddleName != null ? sc.Person.MiddleName + " " : "") + sc.Person.LastName
        });
    }

    private IQueryable<ShipCrew> ApplyFilters(IQueryable<ShipCrew> query, ShipCrewFilter? filters)
    {
        if (filters == null) return query;
        if (filters.ShipId.HasValue) query = Queryable.Where(query, sc => sc.ShipId == filters.ShipId.Value);
        if (filters.PersonId.HasValue) query = Queryable.Where(query, sc => sc.PersonId == filters.PersonId.Value);
        if (!string.IsNullOrEmpty(filters.Role)) query = Queryable.Where(query, sc => sc.Position == filters.Role);
        return query;
    }

    private IQueryable<ShipCrew> GetAllFromDatabase() => Db.ShipCrews.Include(sc => sc.Ship).Include(sc => sc.Person).AsQueryable();
}
