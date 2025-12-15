using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("ShipId", Name = "IX_FishingTrips_Ship")]
public partial class FishingTrip
{
    [Key]
    public int TripId { get; set; }

    public int ShipId { get; set; }

    public int PermitId { get; set; }

    public DateTime DepartureDate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? DeparturePort { get; set; }

    public DateTime? ReturnDate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? ReturnPort { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string TripStatus { get; set; } = null!;

    [InverseProperty("Trip")]
    public virtual ICollection<FishingOperation> FishingOperations { get; set; } = new List<FishingOperation>();

    [InverseProperty("Trip")]
    public virtual ICollection<Landing> Landings { get; set; } = new List<Landing>();

    [ForeignKey("PermitId")]
    [InverseProperty("FishingTrips")]
    public virtual FishingPermit Permit { get; set; } = null!;

    [ForeignKey("ShipId")]
    [InverseProperty("FishingTrips")]
    public virtual Ship Ship { get; set; } = null!;
}
