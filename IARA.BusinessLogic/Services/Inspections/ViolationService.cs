using IARA.DomainModel;
using IARA.DomainModel.DTOs.Inspections;
using IARA.DomainModel.Filters.Inspections;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.Inspections;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.BusinessLogic.Services.Inspections;

public class ViolationService : BaseService, IViolationService
{
    public ViolationService(BaseServiceInjector injector) : base(injector)
    {
    }

    public async Task<IEnumerable<ViolationResponseDTO>> GetAllAsync(BaseFilter<ViolationFilter> filters)
    {
        IQueryable<ViolationResponseDTO> query;
        
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

    public async Task<ViolationResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(v => v.ViolationId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(ViolationRequestDTO violation)
    {
        Violation entity = new Violation()
        {
            InspectionId = violation.InspectionId,
            ViolationType = violation.ViolationType,
            Description = violation.Description,
            FineAmount = violation.FineAmount,
            FineStatus = "Unpaid",
            ViolatorPersonId = violation.ViolatorPersonId,
            ViolatorLegalEntityId = violation.ViolatorLegalEntityId,
            ActNumber = violation.ActNumber
        };

        Db.Violations.Add(entity);
        await Db.SaveChangesAsync();

        var inspection = await Db.Inspections.FindAsync(violation.InspectionId);
        if (inspection != null)
        {
            inspection.ViolationFound = true;
            await Db.SaveChangesAsync();
        }

        return entity.ViolationId;
    }

    public async Task<bool> EditAsync(ViolationRequestDTO violation)
    {
        if (!violation.ViolationId.HasValue)
        {
            throw new ArgumentException("ViolationId is required for edit operation");
        }

        Violation entity = await GetAllFromDatabase()
            .Where(v => v.ViolationId == violation.ViolationId.Value)
            .SingleAsync();

        entity.InspectionId = violation.InspectionId;
        entity.ViolationType = violation.ViolationType;
        entity.Description = violation.Description;
        entity.FineAmount = violation.FineAmount;

        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetAllFromDatabase().Where(v => v.ViolationId == id).SingleAsync();
        Db.Violations.Remove(entity);
        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> MarkAsPaidAsync(int id)
    {
        var violation = await GetAllFromDatabase().Where(v => v.ViolationId == id).SingleAsync();
        violation.FineStatus = "Paid";
        violation.PaymentDate = DateTime.Now;
        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> IssueFineAsync(int id, decimal amount)
    {
        var violation = await GetAllFromDatabase().Where(v => v.ViolationId == id).SingleAsync();
        violation.FineAmount = amount;
        violation.FineStatus = "Unpaid";
        return await Db.SaveChangesAsync() > 0;
    }

    private IQueryable<Violation> ApplyPagination(IQueryable<Violation> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    private IQueryable<Violation> ApplyFreeTextSearch(IQueryable<Violation> query, string text)
    {
        return query.Where(x =>
            x.ViolationType.Contains(text) ||
            (x.Description != null && x.Description.Contains(text)));
    }

    private IQueryable<ViolationResponseDTO> ApplyMapping(IQueryable<Violation> query)
    {
        return (from violation in query
            join inspection in Db.Inspections on violation.InspectionId equals inspection.InspectionId
            join inspector in Db.Inspectors on inspection.InspectorId equals inspector.InspectorId
            join person in Db.Persons on inspector.PersonId equals person.PersonId
            select new ViolationResponseDTO()
            {
                ViolationId = violation.ViolationId,
                InspectionId = violation.InspectionId,
                ViolationType = violation.ViolationType,
                Description = violation.Description ?? "",
                ActNumber = violation.ActNumber,
                FineAmount = violation.FineAmount ?? 0,
                IssuedOn = inspection.InspectionDate,
                IsPaid = violation.FineStatus == "Paid",
                PaidOn = violation.PaymentDate,
                InspectorName = person.FirstName + " " + person.LastName,
                ViolatorPersonId = violation.ViolatorPersonId,
                ViolatorPersonName = violation.ViolatorPerson != null 
                    ? violation.ViolatorPerson.FirstName + " " + violation.ViolatorPerson.LastName 
                    : null,
                ViolatorLegalEntityId = violation.ViolatorLegalEntityId,
                ViolatorLegalEntityName = violation.ViolatorLegalEntity != null 
                    ? violation.ViolatorLegalEntity.Name 
                    : null,
                ViolatorEIK = violation.ViolatorLegalEntity != null 
                    ? violation.ViolatorLegalEntity.EIK 
                    : null
            });
    }

    private IQueryable<Violation> ApplyFilters(IQueryable<Violation> query, ViolationFilter? filters)
    {
        if (filters == null)
        {
            return query;
        }

        if (filters.InspectionId.HasValue)
        {
            query = query.Where(v => v.InspectionId == filters.InspectionId.Value);
        }

        if (filters.IssuedOnFrom.HasValue)
        {
            query = query.Where(v => v.Inspection.InspectionDate >= filters.IssuedOnFrom.Value);
        }

        if (filters.IssuedOnTo.HasValue)
        {
            query = query.Where(v => v.Inspection.InspectionDate <= filters.IssuedOnTo.Value);
        }

        if (!string.IsNullOrEmpty(filters.ViolationType))
        {
            query = query.Where(v => v.ViolationType == filters.ViolationType);
        }

        if (filters.IsPaid.HasValue)
        {
            var status = filters.IsPaid.Value ? "Paid" : "Unpaid";
            query = query.Where(v => v.FineStatus == status);
        }

        return query;
    }

    private IQueryable<Violation> GetAllFromDatabase()
    {
        return Db.Violations
            .Include(v => v.ViolatorPerson)
            .Include(v => v.ViolatorLegalEntity)
            .AsQueryable();
    }
}
