namespace IARA.DomainModel.DTOs.Inspections;

public class InspectionResponseDTO
{
    public int InspectionId { get; set; }
    public string InspectorName { get; set; } = null!;
    public DateTime InspectionDate { get; set; }
    public string InspectionType { get; set; } = null!;
    public int? ShipId { get; set; }
    public string? ShipName { get; set; }
    public int? TransportDocumentId { get; set; }
    public int? RecTicketId { get; set; }
    public string? Location { get; set; }
    public bool ViolationFound { get; set; }
    public string? Notes { get; set; }
    public int ViolationCount { get; set; }
}


