namespace IARA.DomainModel.DTOs.CommercialFishing;

public class ShipEquipmentRequestDTO
{
    public int? EquipmentId { get; set; }
    public int ShipId { get; set; }
    public string EquipmentType { get; set; } = null!;
    public string? EquipmentName { get; set; }
    public int Quantity { get; set; }
    public decimal? Length { get; set; }
    public decimal? MeshSize { get; set; }
    public bool IsActive { get; set; }
}
