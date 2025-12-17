namespace IARA.DomainModel.Filters.RecreationalFishing;

public class RecreationalTicketTypeFilter
{
    public string? Name { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? ValidDays { get; set; }
}

