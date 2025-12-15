namespace IARA.DomainModel.Filters.CommercialFishing;

public class LandingFilter
{
    public int? TripId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public string? Port { get; set; }
    public bool? IsApproved { get; set; }
}

