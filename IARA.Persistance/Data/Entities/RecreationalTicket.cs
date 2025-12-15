using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("QRCode", Name = "UQ__Recreati__5B869AD9310C093F", IsUnique = true)]
[Index("TicketNumber", Name = "UQ__Recreati__CBED06DADBEBD853", IsUnique = true)]
public partial class RecreationalTicket
{
    [Key]
    public int TicketId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string TicketNumber { get; set; } = null!;

    public int RecFishermanId { get; set; }

    public int TicketTypeId { get; set; }

    public DateTime IssuedOn { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidUntil { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Price { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string PurchaseChannel { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string? QRCode { get; set; }

    public bool IsActive { get; set; }

    [InverseProperty("RecTicket")]
    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();

    [ForeignKey("RecFishermanId")]
    [InverseProperty("RecreationalTickets")]
    public virtual RecreationalFisherman RecFisherman { get; set; } = null!;

    [InverseProperty("Ticket")]
    public virtual ICollection<RecreationalCatch> RecreationalCatches { get; set; } = new List<RecreationalCatch>();

    [ForeignKey("TicketTypeId")]
    [InverseProperty("RecreationalTickets")]
    public virtual RecreationalTicketType TicketType { get; set; } = null!;
}
