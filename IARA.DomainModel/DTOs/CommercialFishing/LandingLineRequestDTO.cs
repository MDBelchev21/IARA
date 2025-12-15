namespace IARA.DomainModel.DTOs.CommercialFishing;

public class LandingLineRequestDTO
{
    public int? LandingLineId { get; set; }
    public int LandingId { get; set; }
    public int? CatchId { get; set; }
    public string BatchNumber { get; set; } = null!;
    public string SpeciesName { get; set; } = null!;
    public decimal WeightKg { get; set; }
}
