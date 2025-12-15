using IARA.DomainModel;
using IARA.DomainModel.DTOs.Registry;
using IARA.DomainModel.Filters.Registry;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.Registry;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.BusinessLogic.Services.Registry;

public class PersonService : BaseService, IPersonService
{
    public PersonService(BaseServiceInjector injector) : base(injector)
    {
    }

    public async Task<IEnumerable<PersonResponseDTO>> GetAllAsync(BaseFilter<PersonFilter> filters)
    {
        IQueryable<PersonResponseDTO> query;
        
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

    public async Task<PersonResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(p => p.PersonId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(PersonRequestDTO person)
    {
        Person entity = new Person()
        {
            FirstName = person.FirstName,
            MiddleName = person.MiddleName,
            LastName = person.LastName,
            EGN = person.EGN,
            IdNumber = person.IdNumber,
            DateOfBirth = person.DateOfBirth,
            Email = person.Email,
            Phone = person.Phone,
            Address = person.Address,
            IsDeleted = false
        };

        Db.Persons.Add(entity);
        await Db.SaveChangesAsync();

        return entity.PersonId;
    }

    public async Task<bool> EditAsync(PersonRequestDTO person)
    {
        if (!person.PersonId.HasValue)
        {
            throw new ArgumentException("PersonId is required for edit operation");
        }

        Person entity = await GetAllFromDatabase()
            .Where(p => p.PersonId == person.PersonId.Value)
            .SingleAsync();

        entity.FirstName = person.FirstName;
        entity.MiddleName = person.MiddleName;
        entity.LastName = person.LastName;
        entity.EGN = person.EGN;
        entity.IdNumber = person.IdNumber;
        entity.DateOfBirth = person.DateOfBirth;
        entity.Email = person.Email;
        entity.Phone = person.Phone;
        entity.Address = person.Address;

        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var person = await GetAllFromDatabase().Where(p => p.PersonId == id).SingleAsync();
        person.IsDeleted = true;
        return await Db.SaveChangesAsync() > 0;
    }

    private IQueryable<Person> ApplyPagination(IQueryable<Person> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    private IQueryable<Person> ApplyFreeTextSearch(IQueryable<Person> query, string text)
    {
        return query.Where(x =>
            x.FirstName.Contains(text) ||
            x.LastName.Contains(text) ||
            (x.MiddleName != null && x.MiddleName.Contains(text)) ||
            (x.Email != null && x.Email.Contains(text)) ||
            (x.EGN != null && x.EGN.Contains(text)));
    }

    private IQueryable<PersonResponseDTO> ApplyMapping(IQueryable<Person> query)
    {
        return query.Select(person => new PersonResponseDTO()
        {
            PersonId = person.PersonId,
            FirstName = person.FirstName,
            MiddleName = person.MiddleName,
            LastName = person.LastName,
            FullName = person.FirstName + " " + (person.MiddleName != null ? person.MiddleName + " " : "") + person.LastName,
            EGN = person.EGN,
            IdNumber = person.IdNumber,
            DateOfBirth = person.DateOfBirth,
            Email = person.Email,
            Phone = person.Phone,
            Address = person.Address,
            IsDeleted = person.IsDeleted
        });
    }

    private IQueryable<Person> ApplyFilters(IQueryable<Person> query, PersonFilter? filters)
    {
        query = query.Where(p => !p.IsDeleted);

        if (filters == null)
        {
            return query;
        }

        if (!string.IsNullOrEmpty(filters.FirstName))
        {
            query = query.Where(p => p.FirstName == filters.FirstName);
        }

        if (!string.IsNullOrEmpty(filters.LastName))
        {
            query = query.Where(p => p.LastName == filters.LastName);
        }

        if (!string.IsNullOrEmpty(filters.EGN))
        {
            query = query.Where(p => p.EGN == filters.EGN);
        }

        if (!string.IsNullOrEmpty(filters.Email))
        {
            query = query.Where(p => p.Email == filters.Email);
        }

        if (!string.IsNullOrEmpty(filters.Phone))
        {
            query = query.Where(p => p.Phone == filters.Phone);
        }

        return query;
    }

    private IQueryable<Person> GetAllFromDatabase()
    {
        return Db.Persons.AsQueryable();
    }
}
