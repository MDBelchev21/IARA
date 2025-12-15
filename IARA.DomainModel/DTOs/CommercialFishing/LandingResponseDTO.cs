namespace IARA.DomainModel.DTOs.CommercialFishing;

public class LandingResponseDTO
{
    public int LandingId { get; set; }
    public int TripId { get; set; }
    public DateTime LandingDate { get; set; }
    public string Port { get; set; } = null!;
    public decimal TotalWeight { get; set; }
    public int? ApprovedBy { get; set; }

    public string ShipName { get; set; } = null!;
    public string InternationalNumber { get; set; } = null!;
}

