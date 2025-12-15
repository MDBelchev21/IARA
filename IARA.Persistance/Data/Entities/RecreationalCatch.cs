using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("TicketId", Name = "IX_RecreationalCatches_Ticket")]
public partial class RecreationalCatch
{
    [Key]
    public int RecCatchId { get; set; }

    public int TicketId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string SpeciesName { get; set; } = null!;

    public DateTime CatchDate { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Location { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(6, 3)")]
    public decimal? WeightKg { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string RegisteredVia { get; set; } = null!;

    [ForeignKey("TicketId")]
    [InverseProperty("RecreationalCatches")]
    public virtual RecreationalTicket Ticket { get; set; } = null!;
}
