namespace IARA.DomainModel.DTOs.RecreationalFishing;

public class RecreationalTicketTypeResponseDTO
{
    public int TicketTypeId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int ValidDays { get; set; }
}

