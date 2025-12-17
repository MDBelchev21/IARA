namespace IARA.DomainModel.DTOs.Registry;

public class QualificationRequestDTO
{
    public int? QualificationId { get; set; }
    public int PersonId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime IssuedOn { get; set; }
    public DateTime? ValidUntil { get; set; }
}

