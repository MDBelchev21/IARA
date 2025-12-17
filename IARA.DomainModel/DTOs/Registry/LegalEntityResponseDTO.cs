namespace IARA.DomainModel.DTOs.Registry;

public class LegalEntityResponseDTO
{
    public int LegalEntityId { get; set; }
    public string Name { get; set; } = null!;
    public string EIK { get; set; } = null!;
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool IsDeleted { get; set; }
}

