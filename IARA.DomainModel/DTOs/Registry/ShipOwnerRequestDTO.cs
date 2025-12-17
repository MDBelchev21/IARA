namespace IARA.DomainModel.DTOs.Registry;

public class ShipOwnerRequestDTO
{
    public int? ShipOwnerId { get; set; }
    public int ShipId { get; set; }
    public int? PersonId { get; set; }
    public int? LegalEntityId { get; set; }
    public decimal OwnershipShare { get; set; }
    public DateOnly ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
    public bool IsActive { get; set; } = true;
}

