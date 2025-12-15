namespace IARA.DomainModel.DTOs.CommercialFishing;

public class FishingTripRequestDTO
{
    public int? TripId { get; set; }
    public int ShipId { get; set; }
    public int PermitId { get; set; }
    public DateTime DepartureDate { get; set; }
    public string? DeparturePort { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string? ReturnPort { get; set; }
    public string TripStatus { get; set; } = null!;
}
