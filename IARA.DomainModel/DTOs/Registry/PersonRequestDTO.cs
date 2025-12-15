namespace IARA.DomainModel.DTOs.Registry;

public class PersonRequestDTO
{
    public int? PersonId { get; set; }
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public string? EGN { get; set; }
    public string? IdNumber { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
}

