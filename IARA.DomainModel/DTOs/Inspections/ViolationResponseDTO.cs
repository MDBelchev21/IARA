namespace IARA.DomainModel.DTOs.Inspections;

public class ViolationResponseDTO
{
    public int ViolationId { get; set; }
    public int InspectionId { get; set; }
    public string ViolationType { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? ActNumber { get; set; }
    public decimal FineAmount { get; set; }
    public DateTime IssuedOn { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidOn { get; set; }
    public string InspectorName { get; set; } = null!;
    public int? ViolatorPersonId { get; set; }
    public string? ViolatorPersonName { get; set; }
    public int? ViolatorLegalEntityId { get; set; }
    public string? ViolatorLegalEntityName { get; set; }
    public string? ViolatorEIK { get; set; }
}

