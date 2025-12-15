namespace IARA.DomainModel.DTOs.CommercialFishing;

public class FishingPermitRequestDTO
{
    public int? PermitId { get; set; }
    public int ShipId { get; set; }
    public int OwnerId { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
}

