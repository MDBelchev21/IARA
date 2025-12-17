using IARA.DomainModel;
using IARA.DomainModel.DTOs.Registry;
using IARA.DomainModel.Filters.Registry;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.Registry;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IARA.BusinessLogic.Services.Registry;

public class QualificationService : BaseService, IQualificationService
{
    public QualificationService(BaseServiceInjector injector) : base(injector) { }

    public async Task<IEnumerable<QualificationResponseDTO>> GetAllAsync(BaseFilter<PersonFilter> filters)
    {
        IQueryable<QualificationResponseDTO> query;
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

    public async Task<QualificationResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(q => q.QualificationId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(QualificationRequestDTO entity)
    {
        Qualification dbEntity = new Qualification()
        {
            PersonId = entity.PersonId,
            QualificationType = entity.Name,
            CertificateNumber = Guid.NewGuid().ToString("N").Substring(0, 12),
            IssuedOn = DateOnly.FromDateTime(entity.IssuedOn),
            ValidUntil = entity.ValidUntil.HasValue ? DateOnly.FromDateTime(entity.ValidUntil.Value) : null,
            IsRevoked = false
        };
        Db.Qualifications.Add(dbEntity);
        await Db.SaveChangesAsync();
        return dbEntity.QualificationId;
    }

    public async Task<bool> EditAsync(QualificationRequestDTO entity)
    {
        if (!entity.QualificationId.HasValue) throw new ArgumentException("QualificationId is required");
        var dbEntity = await GetAllFromDatabase().Where(q => q.QualificationId == entity.QualificationId.Value).SingleAsync();
        dbEntity.QualificationType = entity.Name;
        dbEntity.IssuedOn = DateOnly.FromDateTime(entity.IssuedOn);
        dbEntity.ValidUntil = entity.ValidUntil.HasValue ? DateOnly.FromDateTime(entity.ValidUntil.Value) : null;
        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var dbEntity = await GetAllFromDatabase().Where(q => q.QualificationId == id).SingleAsync();
        Db.Qualifications.Remove(dbEntity);
        return await Db.SaveChangesAsync() > 0;
    }

    private IQueryable<Qualification> ApplyPagination(IQueryable<Qualification> query, int page, int pageSize) => Queryable.Skip(query, (page - 1) * pageSize).Take(pageSize);

    private IQueryable<Qualification> ApplyFreeTextSearch(IQueryable<Qualification> query, string text) => Queryable.Where(query, x => x.QualificationType.Contains(text) || x.CertificateNumber.Contains(text));

    private IQueryable<QualificationResponseDTO> ApplyMapping(IQueryable<Qualification> query) => Queryable.Select(query, q => new QualificationResponseDTO { QualificationId = q.QualificationId, PersonId = q.PersonId, Name = q.QualificationType, IssuedOn = new DateTime(q.IssuedOn.Year, q.IssuedOn.Month, q.IssuedOn.Day), ValidUntil = q.ValidUntil.HasValue ? new DateTime(q.ValidUntil.Value.Year, q.ValidUntil.Value.Month, q.ValidUntil.Value.Day) : null });

    private IQueryable<Qualification> ApplyFilters(IQueryable<Qualification> query, PersonFilter? filters)
    {
        if (filters == null) return query;
        if (!string.IsNullOrEmpty(filters.Email)) query = Queryable.Where(query, q => q.Person.Email == filters.Email);
        if (!string.IsNullOrEmpty(filters.EGN)) query = Queryable.Where(query, q => q.Person.EGN == filters.EGN);
        if (!string.IsNullOrEmpty(filters.FirstName)) query = Queryable.Where(query, q => q.Person.FirstName == filters.FirstName);
        if (!string.IsNullOrEmpty(filters.LastName)) query = Queryable.Where(query, q => q.Person.LastName == filters.LastName);
        return query;
    }

    private IQueryable<Qualification> GetAllFromDatabase() => Db.Qualifications.Include(q => q.Person).AsQueryable();
}
