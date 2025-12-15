namespace IARA.DomainModel.DTOs.CommercialFishing;

public class TransportDocumentRequestDTO
{
    public int? DocumentId { get; set; }
    public string DocumentNumber { get; set; } = null!;
    public DateTime TransportDate { get; set; }
    public string? OriginLocation { get; set; }
    public string DestinationLocation { get; set; } = null!;
    public string? VehicleRegistration { get; set; }
    public string? DriverName { get; set; }
    public DateTime? ReceivedOn { get; set; }
}

