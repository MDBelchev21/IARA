namespace IARA.DomainModel.Filters.CommercialFishing;

public class FishingTripFilter
{
    public int? ShipId { get; set; }
    public int? PermitId { get; set; }
    public DateTime? DepartureFrom { get; set; }
    public DateTime? DepartureTo { get; set; }
    public DateTime? ReturnFrom { get; set; }
    public DateTime? ReturnTo { get; set; }
    public string? TripStatus { get; set; }
}

