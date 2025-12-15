namespace IARA.DomainModel.DTOs.CommercialFishing;

public class TransportLineRequestDTO
{
    public int? TransportLineId { get; set; }
    public int DocumentId { get; set; }
    public string BatchNumber { get; set; } = null!;
    public string SpeciesName { get; set; } = null!;
    public decimal WeightKg { get; set; }
}

