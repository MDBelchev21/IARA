namespace IARA.DomainModel.DTOs.RecreationalFishing;

public class RecreationalFishermanResponseDTO
{
    public int RecFishermanId { get; set; }
    public string FullName { get; set; } = null!;
    public string? EGN { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool IsDisabled { get; set; }
    public string? TELKDecisionNumber { get; set; }
    public int ActiveTicketsCount { get; set; }
}

