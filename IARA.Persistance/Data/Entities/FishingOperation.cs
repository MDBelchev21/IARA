using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("TripId", Name = "IX_FishingOperations_Trip")]
public partial class FishingOperation
{
    [Key]
    public int OperationId { get; set; }

    public int TripId { get; set; }

    public int EquipmentId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Location { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal? Longitude { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? DurationHours { get; set; }

    [InverseProperty("Operation")]
    public virtual ICollection<Catch> Catches { get; set; } = new List<Catch>();

    [ForeignKey("EquipmentId")]
    [InverseProperty("FishingOperations")]
    public virtual ShipEquipment Equipment { get; set; } = null!;

    [ForeignKey("TripId")]
    [InverseProperty("FishingOperations")]
    public virtual FishingTrip Trip { get; set; } = null!;
}
