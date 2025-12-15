using IARA.DomainModel;
using IARA.DomainModel.DTOs.Inspections;
using IARA.DomainModel.Filters.Inspections;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.Inspections;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.BusinessLogic.Services.Inspections;

public class InspectionService : BaseService, IInspectionService
{
    public InspectionService(BaseServiceInjector injector) : base(injector)
    {
    }

    public async Task<IEnumerable<InspectionResponseDTO>> GetAllAsync(BaseFilter<InspectionFilter> filters)
    {
        IQueryable<InspectionResponseDTO> query;
        
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

    public async Task<InspectionResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(i => i.InspectionId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(InspectionRequestDTO inspection)
    {
        Inspection entity = new Inspection()
        {
            InspectorId = inspection.InspectorId,
            InspectionDate = inspection.InspectionDate,
            InspectionType = inspection.InspectionType,
            ShipId = inspection.ShipId,
            TransportDocumentId = inspection.TransportDocumentId,
            RecTicketId = inspection.RecTicketId,
            Location = inspection.Location,
            ViolationFound = inspection.ViolationFound,
            Notes = inspection.Notes
        };

        Db.Inspections.Add(entity);
        await Db.SaveChangesAsync();

        return entity.InspectionId;
    }

    public async Task<bool> EditAsync(InspectionRequestDTO inspection)
    {
        if (!inspection.InspectionId.HasValue)
        {
            throw new ArgumentException("InspectionId is required for edit operation");
        }

        Inspection entity = await GetAllFromDatabase()
            .Where(i => i.InspectionId == inspection.InspectionId.Value)
            .SingleAsync();

        entity.InspectorId = inspection.InspectorId;
        entity.InspectionDate = inspection.InspectionDate;
        entity.InspectionType = inspection.InspectionType;
        entity.ShipId = inspection.ShipId;
        entity.TransportDocumentId = inspection.TransportDocumentId;
        entity.RecTicketId = inspection.RecTicketId;
        entity.Location = inspection.Location;
        entity.ViolationFound = inspection.ViolationFound;
        entity.Notes = inspection.Notes;

        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetAllFromDatabase().Where(i => i.InspectionId == id).SingleAsync();
        Db.Inspections.Remove(entity);
        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> CompleteInspectionAsync(int id)
    {
        var inspection = await GetAllFromDatabase().Where(i => i.InspectionId == id).SingleAsync();
        return await Db.SaveChangesAsync() >= 0;
    }

    private IQueryable<Inspection> ApplyPagination(IQueryable<Inspection> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    private IQueryable<Inspection> ApplyFreeTextSearch(IQueryable<Inspection> query, string text)
    {
        return query.Where(x =>
            x.InspectionType.Contains(text) ||
            (x.Location != null && x.Location.Contains(text)) ||
            (x.Notes != null && x.Notes.Contains(text)) ||
            x.Inspector.Person.FirstName.Contains(text) ||
            x.Inspector.Person.LastName.Contains(text));
    }

    private IQueryable<InspectionResponseDTO> ApplyMapping(IQueryable<Inspection> query)
    {
        return (from inspection in query
            join inspector in Db.Inspectors on inspection.InspectorId equals inspector.InspectorId
            join person in Db.Persons on inspector.PersonId equals person.PersonId
            select new InspectionResponseDTO()
            {
                InspectionId = inspection.InspectionId,
                InspectorName = person.FirstName + " " + person.LastName,
                InspectionDate = inspection.InspectionDate,
                InspectionType = inspection.InspectionType,
                ShipId = inspection.ShipId,
                ShipName = inspection.Ship != null ? (inspection.Ship.Name ?? inspection.Ship.ExternalMarking) : null,
                TransportDocumentId = inspection.TransportDocumentId,
                RecTicketId = inspection.RecTicketId,
                Location = inspection.Location,
                ViolationFound = inspection.ViolationFound,
                Notes = inspection.Notes,
                ViolationCount = inspection.Violations.Count
            });
    }

    private IQueryable<Inspection> ApplyFilters(IQueryable<Inspection> query, InspectionFilter? filters)
    {
        if (filters == null)
        {
            return query;
        }

        if (filters.InspectorId.HasValue)
        {
            query = query.Where(i => i.InspectorId == filters.InspectorId.Value);
        }

        if (filters.InspectionDateFrom.HasValue)
        {
            query = query.Where(i => i.InspectionDate >= filters.InspectionDateFrom.Value);
        }

        if (filters.InspectionDateTo.HasValue)
        {
            query = query.Where(i => i.InspectionDate <= filters.InspectionDateTo.Value);
        }

        if (!string.IsNullOrEmpty(filters.InspectionType))
        {
            query = query.Where(i => i.InspectionType == filters.InspectionType);
        }

        if (filters.ShipId.HasValue)
        {
            query = query.Where(i => i.ShipId == filters.ShipId.Value);
        }

        if (filters.ViolationFound.HasValue)
        {
            query = query.Where(i => i.ViolationFound == filters.ViolationFound.Value);
        }

        if (!string.IsNullOrEmpty(filters.Location))
        {
            query = query.Where(i => i.Location == filters.Location);
        }

        return query;
    }

    private IQueryable<Inspection> GetAllFromDatabase()
    {
        return Db.Inspections.AsQueryable();
    }
}
