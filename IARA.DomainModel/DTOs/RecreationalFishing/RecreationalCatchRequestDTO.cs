namespace IARA.DomainModel.DTOs.RecreationalFishing;

public class RecreationalCatchRequestDTO
{
    public int? RecCatchId { get; set; }
    public int TicketId { get; set; }
    public DateTime CatchDate { get; set; }
    public string SpeciesName { get; set; } = null!;
    public decimal? WeightKg { get; set; }
    public string? Location { get; set; }
    public int Quantity { get; set; }
    public string RegisteredVia { get; set; } = "API";
}
