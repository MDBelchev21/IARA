using IARA.DomainModel;
using IARA.DomainModel.DTOs.CommercialFishing;
using IARA.DomainModel.Filters.CommercialFishing;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.CommercialFishing;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.BusinessLogic.Services.CommercialFishing;

public class ShipEquipmentService : BaseService, IShipEquipmentService
{
    public ShipEquipmentService(BaseServiceInjector injector) : base(injector)
    {
    }

    public async Task<IEnumerable<ShipEquipmentResponseDTO>> GetAllAsync(BaseFilter<ShipEquipmentFilter> filters)
    {
        IQueryable<ShipEquipmentResponseDTO> query;
        
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

    public async Task<ShipEquipmentResponseDTO?> GetAsync(int id)
    {
        return await ApplyMapping(GetAllFromDatabase().Where(e => e.EquipmentId == id)).FirstOrDefaultAsync();
    }

    public async Task<int> AddAsync(ShipEquipmentRequestDTO equipment)
    {
        ValidateEquipment(equipment);

        // Verify ship exists
        var shipExists = await Db.Ships.AnyAsync(s => s.ShipId == equipment.ShipId && !s.IsDeleted);
        if (!shipExists)
        {
            throw new InvalidOperationException($"Ship with ID {equipment.ShipId} does not exist.");
        }

        ShipEquipment entity = new ShipEquipment()
        {
            ShipId = equipment.ShipId,
            EquipmentType = equipment.EquipmentType,
            EquipmentName = equipment.EquipmentName,
            Quantity = equipment.Quantity,
            Length = equipment.Length.HasValue ? decimal.Round(equipment.Length.Value, 2, MidpointRounding.AwayFromZero) : null,
            MeshSize = equipment.MeshSize.HasValue ? decimal.Round(equipment.MeshSize.Value, 2, MidpointRounding.AwayFromZero) : null,
            IsActive = equipment.IsActive
        };

        Db.ShipEquipments.Add(entity);
        await Db.SaveChangesAsync();

        return entity.EquipmentId;
    }

    public async Task<bool> EditAsync(ShipEquipmentRequestDTO equipment)
    {
        if (!equipment.EquipmentId.HasValue)
        {
            throw new ArgumentException("EquipmentId is required for edit operation");
        }

        ValidateEquipment(equipment);

        ShipEquipment entity = await GetAllFromDatabase()
            .Where(e => e.EquipmentId == equipment.EquipmentId.Value)
            .SingleAsync();

        entity.ShipId = equipment.ShipId;
        entity.EquipmentType = equipment.EquipmentType;
        entity.EquipmentName = equipment.EquipmentName;
        entity.Quantity = equipment.Quantity;
        entity.Length = equipment.Length.HasValue ? decimal.Round(equipment.Length.Value, 2, MidpointRounding.AwayFromZero) : null;
        entity.MeshSize = equipment.MeshSize.HasValue ? decimal.Round(equipment.MeshSize.Value, 2, MidpointRounding.AwayFromZero) : null;
        entity.IsActive = equipment.IsActive;

        return await Db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var equipment = await GetAllFromDatabase().Where(e => e.EquipmentId == id).SingleAsync();
        equipment.IsActive = false;
        return await Db.SaveChangesAsync() > 0;
    }

    private static void ValidateEquipment(ShipEquipmentRequestDTO equipment)
    {
        if (equipment.Quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(equipment.Quantity), "Quantity must be greater than 0.");
        }
        
        if (equipment.Length.HasValue && equipment.Length.Value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(equipment.Length), "Length cannot be negative.");
        }
        
        if (equipment.MeshSize.HasValue && equipment.MeshSize.Value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(equipment.MeshSize), "Mesh size cannot be negative.");
        }

        if (string.IsNullOrWhiteSpace(equipment.EquipmentType))
        {
            throw new ArgumentException("Equipment type is required.", nameof(equipment.EquipmentType));
        }
    }

    private IQueryable<ShipEquipment> ApplyPagination(IQueryable<ShipEquipment> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    private IQueryable<ShipEquipment> ApplyFreeTextSearch(IQueryable<ShipEquipment> query, string text)
    {
        return query.Where(x =>
            x.EquipmentType.Contains(text) ||
            (x.EquipmentName != null && x.EquipmentName.Contains(text)) ||
            x.Ship.ExternalMarking.Contains(text) ||
            (x.Ship.Name != null && x.Ship.Name.Contains(text)));
    }

    private IQueryable<ShipEquipmentResponseDTO> ApplyMapping(IQueryable<ShipEquipment> query)
    {
        return (from equipment in query
            join ship in Db.Ships on equipment.ShipId equals ship.ShipId
            where !ship.IsDeleted
            select new ShipEquipmentResponseDTO()
            {
                EquipmentId = equipment.EquipmentId,
                ShipId = equipment.ShipId,
                EquipmentType = equipment.EquipmentType,
                EquipmentName = equipment.EquipmentName,
                Quantity = equipment.Quantity,
                Length = equipment.Length,
                MeshSize = equipment.MeshSize,
                IsActive = equipment.IsActive,
                ShipName = ship.Name ?? "",
                ExternalMarking = ship.ExternalMarking
            });
    }

    private IQueryable<ShipEquipment> ApplyFilters(IQueryable<ShipEquipment> query, ShipEquipmentFilter? filters)
    {
        if (filters == null)
        {
            return query;
        }

        if (filters.ShipId.HasValue)
        {
            query = query.Where(e => e.ShipId == filters.ShipId.Value);
        }

        if (!string.IsNullOrEmpty(filters.EquipmentType))
        {
            query = query.Where(e => e.EquipmentType == filters.EquipmentType);
        }

        if (filters.IsActive.HasValue)
        {
            query = query.Where(e => e.IsActive == filters.IsActive.Value);
        }

        return query;
    }

    private IQueryable<ShipEquipment> GetAllFromDatabase()
    {
        return Db.ShipEquipments.AsQueryable();
    }
}
