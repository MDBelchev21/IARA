namespace IARA.DomainModel.Filters.CommercialFishing;

public class FishingPermitFilter
{
    public int? ShipId { get; set; }
    public int? OwnerId { get; set; }
    public DateTime? IssuedOnFrom { get; set; }
    public DateTime? IssuedOnTo { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public bool? IsActive { get; set; }
}

