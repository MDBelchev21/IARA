namespace IARA.DomainModel.DTOs.CommercialFishing;

public class FishingPermitResponseDTO
{
    public int PermitId { get; set; }
    public string PermitNumber { get; set; } = null!;
    public string ShipName { get; set; } = null!;
    public string ShipMarking { get; set; } = null!;
    public string OwnerName { get; set; } = null!;
    public DateTime IssuedOn { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
    public bool IsActive { get; set; }
    public int EquipmentCount { get; set; }
}

