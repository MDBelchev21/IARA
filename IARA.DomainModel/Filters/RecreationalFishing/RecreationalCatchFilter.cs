namespace IARA.DomainModel.Filters.RecreationalFishing;

public class RecreationalCatchFilter
{
    public int? TicketId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? SpeciesName { get; set; }
}
