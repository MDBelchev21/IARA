using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("BatchNumber", Name = "IX_LandingLines_Batch")]
[Index("BatchNumber", Name = "UQ__LandingL__F869ED6D49D901A9", IsUnique = true)]
public partial class LandingLine
{
    [Key]
    public int LandingLineId { get; set; }

    public int LandingId { get; set; }

    public int? CatchId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string BatchNumber { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string SpeciesName { get; set; } = null!;

    [Column(TypeName = "decimal(10, 3)")]
    public decimal WeightKg { get; set; }

    [ForeignKey("CatchId")]
    [InverseProperty("LandingLines")]
    public virtual Catch? Catch { get; set; }

    [ForeignKey("LandingId")]
    [InverseProperty("LandingLines")]
    public virtual Landing Landing { get; set; } = null!;
}
