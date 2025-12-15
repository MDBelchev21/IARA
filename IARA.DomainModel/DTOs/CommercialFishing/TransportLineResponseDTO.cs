namespace IARA.DomainModel.DTOs.CommercialFishing;

public class TransportLineResponseDTO
{
    public int TransportLineId { get; set; }
    public int DocumentId { get; set; }
    public string BatchNumber { get; set; } = null!;
    public string SpeciesName { get; set; } = null!;
    public decimal WeightKg { get; set; }
}

