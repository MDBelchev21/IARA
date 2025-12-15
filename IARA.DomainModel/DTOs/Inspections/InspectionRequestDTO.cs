namespace IARA.DomainModel.DTOs.Inspections;

public class InspectionRequestDTO
{
    public int? InspectionId { get; set; }
    public int InspectorId { get; set; }
    public DateTime InspectionDate { get; set; }
    public string InspectionType { get; set; } = null!;
    public int? ShipId { get; set; }
    public int? TransportDocumentId { get; set; }
    public int? RecTicketId { get; set; }
    public string? Location { get; set; }
    public bool ViolationFound { get; set; }
    public string? Notes { get; set; }
}

