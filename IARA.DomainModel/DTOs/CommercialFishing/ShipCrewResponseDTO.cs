namespace IARA.DomainModel.DTOs.CommercialFishing;

public class ShipCrewResponseDTO
{
    public int ShipCrewId { get; set; }
    public int ShipId { get; set; }
    public int PersonId { get; set; }
    public string Position { get; set; } = null!;
    public bool IsCaptain { get; set; }
    public int? QualificationId { get; set; }
    public DateOnly AssignedOn { get; set; }
    public DateOnly? RelievedOn { get; set; }
    public bool IsActive { get; set; }
    public string ShipName { get; set; } = null!;
    public string PersonFullName { get; set; } = null!;
}
