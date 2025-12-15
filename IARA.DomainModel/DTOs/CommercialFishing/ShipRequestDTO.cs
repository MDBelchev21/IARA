namespace IARA.DomainModel.DTOs.CommercialFishing;

public class ShipRequestDTO
{
    public int? ShipId { get; set; }
    public string? InternationalNumber { get; set; }
    public string? RadioCallSign { get; set; }
    public string ExternalMarking { get; set; } = null!;
    public string? Name { get; set; }
    public decimal Length { get; set; }
    public decimal Width { get; set; }
    public decimal? GrossTonnage { get; set; }
    public decimal? Draft { get; set; }
    public decimal? MainEnginePower { get; set; }
    public string? FuelType { get; set; }
    public decimal? FuelCapacity { get; set; }
    public int OwnerId { get; set; }
}

