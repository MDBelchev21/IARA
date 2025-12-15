using IARA.DomainModel;
using IARA.DomainModel.DTOs.RecreationalFishing;
using IARA.DomainModel.Filters.RecreationalFishing;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.RecreationalFishing;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.BusinessLogic.Services.RecreationalFishing;

public class RecreationalTicketService : BaseService, IRecreationalTicketService
{
    public RecreationalTicketService(BaseServiceInjector injector) : base(injector)
    {
    }

    public async Task<IEnumerable<RecreationalTicketResponseDTO>> GetAllAsync(BaseFilter<RecreationalTicketFilter> filters)
    {
        IQueryable<RecreationalTicketResponseDTO> query;
        
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

    public async Task<RecreationalTicketResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(t => t.TicketId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(RecreationalTicketRequestDTO ticket)
    {
        var ticketType = await Db.RecreationalTicketTypes.FirstOrDefaultAsync(tt => tt.TicketTypeId == ticket.TicketTypeId);
        if (ticketType == null)
        {
            throw new ArgumentException("Invalid ticket type");
        }

        RecreationalTicket entity = new RecreationalTicket()
        {
            TicketNumber = GenerateTicketNumber(),
            RecFishermanId = ticket.RecFishermanId,
            TicketTypeId = ticket.TicketTypeId,
            IssuedOn = DateTime.Now,
            ValidFrom = ticket.ValidFrom,
            ValidUntil = ticket.ValidUntil,
            Price = ticketType.PriceAdult, // Default to adult price, should be calculated based on fisherman type
            PurchaseChannel = ticket.PurchaseChannel,
            QRCode = GenerateQRCode(),
            IsActive = true
        };

        Db.RecreationalTickets.Add(entity);
        await Db.SaveChangesAsync();

        return entity.TicketId;
    }

    public async Task<bool> EditAsync(RecreationalTicketRequestDTO ticket)
    {
        if (!ticket.TicketId.HasValue)
        {
            throw new ArgumentException("TicketId is required for edit operation");
        }

        RecreationalTicket entity = await GetAllFromDatabase()
            .Where(t => t.TicketId == ticket.TicketId.Value)
            .SingleAsync();

        entity.RecFishermanId = ticket.RecFishermanId;
        entity.TicketTypeId = ticket.TicketTypeId;
        entity.ValidFrom = ticket.ValidFrom;
        entity.ValidUntil = ticket.ValidUntil;
        entity.PurchaseChannel = ticket.PurchaseChannel;

        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetAllFromDatabase().Where(t => t.TicketId == id).SingleAsync();
        Db.RecreationalTickets.Remove(entity);
        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeactivateTicketAsync(int id)
    {
        var ticket = await GetAllFromDatabase().Where(t => t.TicketId == id).SingleAsync();
        ticket.IsActive = false;
        return await Db.SaveChangesAsync() > 0;
    }

    private IQueryable<RecreationalTicket> ApplyPagination(IQueryable<RecreationalTicket> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    private IQueryable<RecreationalTicket> ApplyFreeTextSearch(IQueryable<RecreationalTicket> query, string text)
    {
        return query.Where(x => 
            x.TicketNumber.Contains(text) || 
            x.RecFisherman.Person.FirstName.Contains(text) ||
            x.RecFisherman.Person.LastName.Contains(text));
    }

    private IQueryable<RecreationalTicketResponseDTO> ApplyMapping(IQueryable<RecreationalTicket> query)
    {
        return (from ticket in query
            join fisherman in Db.RecreationalFishermen on ticket.RecFishermanId equals fisherman.RecFishermanId
            join person in Db.Persons on fisherman.PersonId equals person.PersonId
            join ticketType in Db.RecreationalTicketTypes on ticket.TicketTypeId equals ticketType.TicketTypeId
            select new RecreationalTicketResponseDTO()
            {
                TicketId = ticket.TicketId,
                TicketNumber = ticket.TicketNumber,
                FishermanName = person.FirstName + " " + person.LastName,
                FishermanEGN = person.EGN ?? "",
                TicketTypeName = ticketType.TypeName,
                IssuedOn = ticket.IssuedOn,
                ValidFrom = ticket.ValidFrom,
                ValidUntil = ticket.ValidUntil,
                Price = ticket.Price,
                PurchaseChannel = ticket.PurchaseChannel,
                QRCode = ticket.QRCode,
                IsActive = ticket.IsActive
            });
    }

    private IQueryable<RecreationalTicket> ApplyFilters(IQueryable<RecreationalTicket> query, RecreationalTicketFilter? filters)
    {
        if (filters == null)
        {
            return query;
        }

        if (!string.IsNullOrEmpty(filters.TicketNumber))
        {
            query = query.Where(t => t.TicketNumber == filters.TicketNumber);
        }

        if (filters.RecFishermanId.HasValue)
        {
            query = query.Where(t => t.RecFishermanId == filters.RecFishermanId.Value);
        }

        if (filters.IssuedOnFrom.HasValue)
        {
            query = query.Where(t => t.IssuedOn >= filters.IssuedOnFrom.Value);
        }

        if (filters.IssuedOnTo.HasValue)
        {
            query = query.Where(t => t.IssuedOn <= filters.IssuedOnTo.Value);
        }

        if (filters.ValidFrom.HasValue)
        {
            query = query.Where(t => t.ValidFrom >= filters.ValidFrom.Value);
        }

        if (filters.ValidTo.HasValue)
        {
            query = query.Where(t => t.ValidUntil <= filters.ValidTo.Value);
        }

        if (filters.IsActive.HasValue)
        {
            query = query.Where(t => t.IsActive == filters.IsActive.Value);
        }

        if (!string.IsNullOrEmpty(filters.PurchaseChannel))
        {
            query = query.Where(t => t.PurchaseChannel == filters.PurchaseChannel);
        }

        return query;
    }

    private IQueryable<RecreationalTicket> GetAllFromDatabase()
    {
        return Db.RecreationalTickets.AsQueryable();
    }

    private string GenerateTicketNumber()
    {
        return $"RT-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }

    private string GenerateQRCode()
    {
        return Guid.NewGuid().ToString();
    }
}

