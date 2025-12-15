using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("TripId", Name = "IX_Landings_Trip")]
public partial class Landing
{
    [Key]
    public int LandingId { get; set; }

    public int TripId { get; set; }

    public DateTime LandingDate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Port { get; set; } = null!;

    [Column(TypeName = "decimal(10, 3)")]
    public decimal TotalWeight { get; set; }

    public int? ApprovedBy { get; set; }

    [InverseProperty("Landing")]
    public virtual ICollection<LandingLine> LandingLines { get; set; } = new List<LandingLine>();

    [ForeignKey("TripId")]
    [InverseProperty("Landings")]
    public virtual FishingTrip Trip { get; set; } = null!;
}
