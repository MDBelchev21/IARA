namespace IARA.DomainModel.Filters.RecreationalFishing;

public class RecreationalTicketFilter
{
    public string? TicketNumber { get; set; }
    public int? RecFishermanId { get; set; }
    public DateTime? IssuedOnFrom { get; set; }
    public DateTime? IssuedOnTo { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public bool? IsActive { get; set; }
    public string? PurchaseChannel { get; set; }
}

