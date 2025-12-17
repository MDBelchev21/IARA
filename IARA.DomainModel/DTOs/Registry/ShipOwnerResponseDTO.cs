namespace IARA.DomainModel.DTOs.Registry;

public class ShipOwnerResponseDTO
{
    public int ShipOwnerId { get; set; }
    public int ShipId { get; set; }
    public int? PersonId { get; set; }
    public int? LegalEntityId { get; set; }
    public decimal OwnershipShare { get; set; }
    public DateOnly ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
    public bool IsActive { get; set; }

    public string ShipName { get; set; } = null!;
    public string? PersonFullName { get; set; }
    public string? LegalEntityName { get; set; }
}

