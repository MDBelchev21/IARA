namespace IARA.DomainModel.Filters.Inspections;

public class ViolationFilter
{
    public int? InspectionId { get; set; }
    public DateTime? IssuedOnFrom { get; set; }
    public DateTime? IssuedOnTo { get; set; }
    public string? ViolationType { get; set; }
    public bool? IsPaid { get; set; }
}

