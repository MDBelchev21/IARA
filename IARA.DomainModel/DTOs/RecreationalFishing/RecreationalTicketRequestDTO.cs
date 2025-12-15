namespace IARA.DomainModel.DTOs.RecreationalFishing;

public class RecreationalTicketRequestDTO
{
    public int? TicketId { get; set; }
    public int RecFishermanId { get; set; }
    public int TicketTypeId { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
    public string PurchaseChannel { get; set; } = null!;
}

