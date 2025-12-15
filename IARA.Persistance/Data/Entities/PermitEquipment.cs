using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Table("PermitEquipment")]
[Index("PermitId", "EquipmentId", Name = "UK_PermitEquipment", IsUnique = true)]
public partial class PermitEquipment
{
    [Key]
    public int PermitEquipmentId { get; set; }

    public int PermitId { get; set; }

    public int EquipmentId { get; set; }

    [ForeignKey("EquipmentId")]
    [InverseProperty("PermitEquipments")]
    public virtual ShipEquipment Equipment { get; set; } = null!;

    [ForeignKey("PermitId")]
    [InverseProperty("PermitEquipments")]
    public virtual FishingPermit Permit { get; set; } = null!;
}
