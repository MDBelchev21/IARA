using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("ValidFrom", "ValidUntil", Name = "IX_FishingPermits_ValidDates")]
[Index("PermitNumber", Name = "UQ__FishingP__DA3C94EEAF67ECA5", IsUnique = true)]
public partial class FishingPermit
{
    [Key]
    public int PermitId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PermitNumber { get; set; } = null!;

    public int ShipId { get; set; }

    public DateOnly IssuedOn { get; set; }

    public DateOnly ValidFrom { get; set; }

    public DateOnly ValidUntil { get; set; }

    public bool IsRevoked { get; set; }

    [InverseProperty("Permit")]
    public virtual ICollection<FishingTrip> FishingTrips { get; set; } = new List<FishingTrip>();

    [InverseProperty("Permit")]
    public virtual ICollection<PermitEquipment> PermitEquipments { get; set; } = new List<PermitEquipment>();

    [ForeignKey("ShipId")]
    [InverseProperty("FishingPermits")]
    public virtual Ship Ship { get; set; } = null!;
}
