namespace IARA.DomainModel.DTOs.Inspections;

public class ViolationRequestDTO
{
    public int? ViolationId { get; set; }
    public int InspectionId { get; set; }
    public string ViolationType { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal FineAmount { get; set; }
    public int? ViolatorPersonId { get; set; }
    public int? ViolatorLegalEntityId { get; set; }
    public string? ActNumber { get; set; }
}

