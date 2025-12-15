namespace IARA.DomainModel.Filters.Inspections;

public class InspectionFilter
{
    public int? InspectorId { get; set; }
    public DateTime? InspectionDateFrom { get; set; }
    public DateTime? InspectionDateTo { get; set; }
    public string? InspectionType { get; set; }
    public int? ShipId { get; set; }
    public bool? ViolationFound { get; set; }
    public string? Location { get; set; }
}

