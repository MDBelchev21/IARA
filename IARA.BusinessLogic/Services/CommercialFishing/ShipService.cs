using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.CommercialFishing;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.BusinessLogic.Services.CommercialFishing;

public class ShipService : BaseService, IShipService
{
    public ShipService(BaseServiceInjector injector) : base(injector)
    {
    }

    public async Task<IEnumerable<ShipResponseDTO>> GetAllAsync(BaseFilter<ShipFilter> filters)
    {
        IQueryable<ShipResponseDTO> query;
        
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

    public async Task<ShipResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(s => s.ShipId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(ShipRequestDTO ship)
    {
        ValidateDimensions(ship);

        Ship entity = new Ship()
        {
            InternationalNumber = ship.InternationalNumber,
            RadioCallSign = ship.RadioCallSign,
            ExternalMarking = ship.ExternalMarking,
            Name = ship.Name,
            Length = decimal.Round(ship.Length, 2, MidpointRounding.AwayFromZero),
            Width = decimal.Round(ship.Width, 2, MidpointRounding.AwayFromZero),
            GrossTonnage = ship.GrossTonnage.HasValue ? decimal.Round(ship.GrossTonnage.Value, 2, MidpointRounding.AwayFromZero) : null,
            Draft = ship.Draft.HasValue ? decimal.Round(ship.Draft.Value, 2, MidpointRounding.AwayFromZero) : null,
            MainEnginePower = ship.MainEnginePower.HasValue ? decimal.Round(ship.MainEnginePower.Value, 2, MidpointRounding.AwayFromZero) : null,
            FuelType = ship.FuelType,
            FuelCapacity = ship.FuelCapacity.HasValue ? decimal.Round(ship.FuelCapacity.Value, 2, MidpointRounding.AwayFromZero) : null,
            IsDeleted = false
        };

        Db.Ships.Add(entity);
        await Db.SaveChangesAsync();

        Db.ShipOwners.Add(new ShipOwner
        {
            ShipId = entity.ShipId,
            PersonId = ship.OwnerId,
            OwnershipShare = 100,
            ValidFrom = DateOnly.FromDateTime(DateTime.Now),
            IsActive = true
        });

        await Db.SaveChangesAsync();

        return entity.ShipId;
    }

    public async Task<bool> EditAsync(ShipRequestDTO ship)
    {
        if (!ship.ShipId.HasValue)
        {
            throw new ArgumentException("ShipId is required for edit operation");
        }

        ValidateDimensions(ship);

        Ship entity = await GetAllFromDatabase()
            .Where(s => s.ShipId == ship.ShipId.Value)
            .SingleAsync();

        entity.InternationalNumber = ship.InternationalNumber;
        entity.RadioCallSign = ship.RadioCallSign;
        entity.ExternalMarking = ship.ExternalMarking;
        entity.Name = ship.Name;
        entity.Length = decimal.Round(ship.Length, 2, MidpointRounding.AwayFromZero);
        entity.Width = decimal.Round(ship.Width, 2, MidpointRounding.AwayFromZero);
        entity.GrossTonnage = ship.GrossTonnage.HasValue ? decimal.Round(ship.GrossTonnage.Value, 2, MidpointRounding.AwayFromZero) : null;
        entity.Draft = ship.Draft.HasValue ? decimal.Round(ship.Draft.Value, 2, MidpointRounding.AwayFromZero) : null;
        entity.MainEnginePower = ship.MainEnginePower.HasValue ? decimal.Round(ship.MainEnginePower.Value, 2, MidpointRounding.AwayFromZero) : null;
        entity.FuelType = ship.FuelType;
        entity.FuelCapacity = ship.FuelCapacity.HasValue ? decimal.Round(ship.FuelCapacity.Value, 2, MidpointRounding.AwayFromZero) : null;

        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ship = await GetAllFromDatabase().Where(s => s.ShipId == id).SingleAsync();
        ship.IsDeleted = true;
        return await Db.SaveChangesAsync() > 0;
    }

    private static void ValidateDimensions(ShipRequestDTO ship)
    {
        if (ship.Length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(ship.Length), "Length must be greater than 0.");
        }
        if (ship.Width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(ship.Width), "Width must be greater than 0.");
        }
        if (ship.Draft.HasValue && ship.Draft.Value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(ship.Draft), "Draft cannot be negative.");
        }
        if (ship.MainEnginePower.HasValue && ship.MainEnginePower.Value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(ship.MainEnginePower), "Engine power cannot be negative.");
        }
        if (ship.FuelCapacity.HasValue && ship.FuelCapacity.Value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(ship.FuelCapacity), "Fuel capacity cannot be negative.");
        }
    }

    private IQueryable<Ship> ApplyPagination(IQueryable<Ship> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    private IQueryable<Ship> ApplyFreeTextSearch(IQueryable<Ship> query, string text)
    {
        return query.Where(x =>
            (x.Name != null && x.Name.Contains(text)) ||
            (x.InternationalNumber != null && x.InternationalNumber.Contains(text)) ||
            x.ExternalMarking.Contains(text) ||
            (x.RadioCallSign != null && x.RadioCallSign.Contains(text)));
    }

    private IQueryable<ShipResponseDTO> ApplyMapping(IQueryable<Ship> query)
    {
        return (from ship in query
            join shipOwner in Db.ShipOwners on ship.ShipId equals shipOwner.ShipId
            join person in Db.Persons on shipOwner.PersonId equals person.PersonId
            where shipOwner.IsActive && !ship.IsDeleted
            select new ShipResponseDTO()
            {
                ShipId = ship.ShipId,
                InternationalNumber = ship.InternationalNumber,
                RadioCallSign = ship.RadioCallSign,
                ExternalMarking = ship.ExternalMarking,
                Name = ship.Name,
                Length = ship.Length,
                Width = ship.Width,
                GrossTonnage = ship.GrossTonnage,
                Draft = ship.Draft,
                MainEnginePower = ship.MainEnginePower,
                FuelType = ship.FuelType,
                FuelCapacity = ship.FuelCapacity,
                OwnerName = person.FirstName + " " + person.LastName,
                ActivePermitsCount = ship.FishingPermits.Count(p => !p.IsRevoked)
            });
    }

    private IQueryable<Ship> ApplyFilters(IQueryable<Ship> query, ShipFilter? filters)
    {
        query = query.Where(s => !s.IsDeleted);

        if (filters == null)
        {
            return query;
        }

        if (!string.IsNullOrEmpty(filters.Name))
        {
            query = query.Where(s => s.Name == filters.Name);
        }

        if (!string.IsNullOrEmpty(filters.InternationalNumber))
        {
            query = query.Where(s => s.InternationalNumber == filters.InternationalNumber);
        }

        if (!string.IsNullOrEmpty(filters.ExternalMarking))
        {
            query = query.Where(s => s.ExternalMarking == filters.ExternalMarking);
        }

        if (!string.IsNullOrEmpty(filters.RadioCallSign))
        {
            query = query.Where(s => s.RadioCallSign == filters.RadioCallSign);
        }

        if (filters.OwnerId.HasValue)
        {
            query = query.Where(s => s.ShipOwners.Any(so => so.PersonId == filters.OwnerId.Value && so.IsActive));
        }

        return query;
    }

    private IQueryable<Ship> GetAllFromDatabase()
    {
        return Db.Ships.AsQueryable();
    }
}
