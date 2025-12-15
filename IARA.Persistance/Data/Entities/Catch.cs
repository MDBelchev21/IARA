using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("OperationId", Name = "IX_Catches_Operation")]
public partial class Catch
{
    [Key]
    public int CatchId { get; set; }

    public int OperationId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string SpeciesName { get; set; } = null!;

    [Column(TypeName = "decimal(10, 3)")]
    public decimal WeightKg { get; set; }

    [InverseProperty("Catch")]
    public virtual ICollection<LandingLine> LandingLines { get; set; } = new List<LandingLine>();

    [ForeignKey("OperationId")]
    [InverseProperty("Catches")]
    public virtual FishingOperation Operation { get; set; } = null!;
}
