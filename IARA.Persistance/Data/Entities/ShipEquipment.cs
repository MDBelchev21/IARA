using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Table("ShipEquipment")]
public partial class ShipEquipment
{
    [Key]
    public int EquipmentId { get; set; }

    public int ShipId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string EquipmentType { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? EquipmentName { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? Length { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? MeshSize { get; set; }

    public bool IsActive { get; set; }

    [InverseProperty("Equipment")]
    public virtual ICollection<FishingOperation> FishingOperations { get; set; } = new List<FishingOperation>();

    [InverseProperty("Equipment")]
    public virtual ICollection<PermitEquipment> PermitEquipments { get; set; } = new List<PermitEquipment>();

    [ForeignKey("ShipId")]
    [InverseProperty("ShipEquipments")]
    public virtual Ship Ship { get; set; } = null!;
}
