namespace IARA.DomainModel.Filters.CommercialFishing;

public class TransportDocumentFilter
{
    public string? DocumentNumber { get; set; }
    public DateTime? TransportDateFrom { get; set; }
    public DateTime? TransportDateTo { get; set; }
    public string? OriginLocation { get; set; }
    public string? DestinationLocation { get; set; }
    public bool? IsReceived { get; set; }
}
