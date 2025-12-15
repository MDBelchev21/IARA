using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.CommercialFishing;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.BusinessLogic.Services.CommercialFishing;

public class FishingTripService : BaseService, IFishingTripService
{
    public FishingTripService(BaseServiceInjector injector) : base(injector)
    {
    }

    public async Task<IEnumerable<FishingTripResponseDTO>> GetAllAsync(BaseFilter<FishingTripFilter> filters)
    {
        IQueryable<FishingTripResponseDTO> query;

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

    public async Task<FishingTripResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(t => t.TripId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(FishingTripRequestDTO trip)
    {
        FishingTrip entity = new FishingTrip
        {
            ShipId = trip.ShipId,
            PermitId = trip.PermitId,
            DepartureDate = trip.DepartureDate,
            DeparturePort = trip.DeparturePort,
            ReturnDate = trip.ReturnDate,
            ReturnPort = trip.ReturnPort,
            TripStatus = trip.TripStatus
        };

        Db.FishingTrips.Add(entity);
        await Db.SaveChangesAsync();

        return entity.TripId;
    }

    public async Task<bool> EditAsync(FishingTripRequestDTO trip)
    {
        if (!trip.TripId.HasValue)
        {
            throw new ArgumentException("TripId is required for edit operation");
        }

        FishingTrip entity = await GetAllFromDatabase().Where(t => t.TripId == trip.TripId.Value).SingleAsync();

        entity.ShipId = trip.ShipId;
        entity.PermitId = trip.PermitId;
        entity.DepartureDate = trip.DepartureDate;
        entity.DeparturePort = trip.DeparturePort;
        entity.ReturnDate = trip.ReturnDate;
        entity.ReturnPort = trip.ReturnPort;
        entity.TripStatus = trip.TripStatus;

        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        FishingTrip entity = await GetAllFromDatabase().Where(t => t.TripId == id).SingleAsync();
        Db.FishingTrips.Remove(entity);
        return await Db.SaveChangesAsync() > 0;
    }

    private IQueryable<FishingTrip> ApplyPagination(IQueryable<FishingTrip> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    private IQueryable<FishingTrip> ApplyFreeTextSearch(IQueryable<FishingTrip> query, string text)
    {
        return query.Where(x =>
            (x.DeparturePort != null && x.DeparturePort.Contains(text)) ||
            (x.ReturnPort != null && x.ReturnPort.Contains(text)) ||
            x.TripStatus.Contains(text));
    }

    private IQueryable<FishingTripResponseDTO> ApplyMapping(IQueryable<FishingTrip> query)
    {
        return from trip in query
               join ship in Db.Ships on trip.ShipId equals ship.ShipId
               join permit in Db.FishingPermits on trip.PermitId equals permit.PermitId
               select new FishingTripResponseDTO
               {
                   TripId = trip.TripId,
                   ShipId = trip.ShipId,
                   PermitId = trip.PermitId,
                   DepartureDate = trip.DepartureDate,
                   DeparturePort = trip.DeparturePort,
                   ReturnDate = trip.ReturnDate,
                   ReturnPort = trip.ReturnPort,
                   TripStatus = trip.TripStatus,
                   ShipName = ship.Name!,
                   InternationalNumber = ship.InternationalNumber!,
                   PermitNumber = permit.PermitNumber!
               };
    }

    private IQueryable<FishingTrip> ApplyFilters(IQueryable<FishingTrip> query, FishingTripFilter? filters)
    {
        if (filters == null)
        {
            return query;
        }

        if (filters.ShipId.HasValue)
        {
            query = query.Where(t => t.ShipId == filters.ShipId.Value);
        }

        if (filters.PermitId.HasValue)
        {
            query = query.Where(t => t.PermitId == filters.PermitId.Value);
        }

        if (!string.IsNullOrEmpty(filters.TripStatus))
        {
            query = query.Where(t => t.TripStatus == filters.TripStatus);
        }

        if (filters.DepartureFrom.HasValue)
        {
            query = query.Where(t => t.DepartureDate >= filters.DepartureFrom.Value);
        }

        if (filters.DepartureTo.HasValue)
        {
            query = query.Where(t => t.DepartureDate <= filters.DepartureTo.Value);
        }

        if (filters.ReturnFrom.HasValue)
        {
            query = query.Where(t => t.ReturnDate >= filters.ReturnFrom.Value);
        }

        if (filters.ReturnTo.HasValue)
        {
            query = query.Where(t => t.ReturnDate <= filters.ReturnTo.Value);
        }

        return query;
    }

    private IQueryable<FishingTrip> GetAllFromDatabase()
    {
        return Db.FishingTrips.AsQueryable();
    }
}

