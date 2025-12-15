using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.CommercialFishing;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.BusinessLogic.Services.CommercialFishing;

public class LandingService : BaseService, ILandingService
{
    public LandingService(BaseServiceInjector injector) : base(injector)
    {
    }

    public async Task<IEnumerable<LandingResponseDTO>> GetAllAsync(BaseFilter<LandingFilter> filters)
    {
        IQueryable<Landing> query = ApplyFilters(GetAllFromDatabase(), filters.Filters);

        if (!string.IsNullOrEmpty(filters.FreeTextSearch))
        {
            query = ApplyFreeTextSearch(query, filters.FreeTextSearch);
        }

        query = ApplyPagination(query, filters.Page, filters.PageSize);

        return await ApplyMapping(query).ToListAsync();
    }

    public async Task<LandingResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(l => l.LandingId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(LandingRequestDTO landing)
    {
        Landing entity = new Landing
        {
            TripId = landing.TripId,
            LandingDate = landing.LandingDate,
            Port = landing.Port,
            TotalWeight = landing.TotalWeight,
            ApprovedBy = landing.ApprovedBy
        };

        Db.Landings.Add(entity);
        await Db.SaveChangesAsync();

        return entity.LandingId;
    }

    public async Task<bool> EditAsync(LandingRequestDTO landing)
    {
        if (!landing.LandingId.HasValue)
        {
            throw new ArgumentException("LandingId is required for edit operation");
        }

        Landing entity = await GetAllFromDatabase().Where(l => l.LandingId == landing.LandingId.Value).SingleAsync();

        entity.TripId = landing.TripId;
        entity.LandingDate = landing.LandingDate;
        entity.Port = landing.Port;
        entity.TotalWeight = landing.TotalWeight;
        entity.ApprovedBy = landing.ApprovedBy;

        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Landing entity = await GetAllFromDatabase().Where(l => l.LandingId == id).SingleAsync();
        Db.Landings.Remove(entity);
        return await Db.SaveChangesAsync() > 0;
    }

    private IQueryable<Landing> ApplyPagination(IQueryable<Landing> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    private IQueryable<Landing> ApplyFreeTextSearch(IQueryable<Landing> query, string text)
    {
        return query.Where(x => x.Port.Contains(text));
    }

    private IQueryable<LandingResponseDTO> ApplyMapping(IQueryable<Landing> query)
    {
        return from landing in query
               join trip in Db.FishingTrips on landing.TripId equals trip.TripId
               join ship in Db.Ships on trip.ShipId equals ship.ShipId
               select new LandingResponseDTO
               {
                   LandingId = landing.LandingId,
                   TripId = landing.TripId,
                   LandingDate = landing.LandingDate,
                   Port = landing.Port,
                   TotalWeight = landing.TotalWeight,
                   ApprovedBy = landing.ApprovedBy,
                   ShipName = ship.Name!,
                   InternationalNumber = ship.InternationalNumber!
               };
    }

    private IQueryable<Landing> ApplyFilters(IQueryable<Landing> query, LandingFilter? filters)
    {
        if (filters == null)
        {
            return query;
        }

        if (filters.TripId.HasValue)
        {
            query = query.Where(l => l.TripId == filters.TripId.Value);
        }

        if (!string.IsNullOrEmpty(filters.Port))
        {
            query = query.Where(l => l.Port == filters.Port);
        }

        if (filters.DateFrom.HasValue)
        {
            query = query.Where(l => l.LandingDate >= filters.DateFrom.Value);
        }

        if (filters.DateTo.HasValue)
        {
            query = query.Where(l => l.LandingDate <= filters.DateTo.Value);
        }

        if (filters.IsApproved.HasValue)
        {
            if (filters.IsApproved.Value)
            {
                query = query.Where(l => l.ApprovedBy != null);
            }
            else
            {
                query = query.Where(l => l.ApprovedBy == null);
            }
        }

        return query;
    }

    private IQueryable<Landing> GetAllFromDatabase()
    {
        return Db.Landings.AsQueryable();
    }
}
