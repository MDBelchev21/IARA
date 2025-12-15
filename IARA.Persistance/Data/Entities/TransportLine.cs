using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("BatchNumber", Name = "IX_TransportLines_Batch")]
public partial class TransportLine
{
    [Key]
    public int TransportLineId { get; set; }

    public int DocumentId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string BatchNumber { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string SpeciesName { get; set; } = null!;

    [Column(TypeName = "decimal(10, 3)")]
    public decimal WeightKg { get; set; }

    [ForeignKey("DocumentId")]
    [InverseProperty("TransportLines")]
    public virtual TransportDocument Document { get; set; } = null!;
}
