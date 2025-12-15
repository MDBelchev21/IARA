namespace IARA.DomainModel.DTOs.RecreationalFishing;

public class RecreationalTicketResponseDTO
{
    public int TicketId { get; set; }
    public string TicketNumber { get; set; } = null!;
    public string FishermanName { get; set; } = null!;
    public string FishermanEGN { get; set; } = null!;
    public string TicketTypeName { get; set; } = null!;
    public DateTime IssuedOn { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
    public decimal Price { get; set; }
    public string PurchaseChannel { get; set; } = null!;
    public string? QRCode { get; set; }
    public bool IsActive { get; set; }
}


