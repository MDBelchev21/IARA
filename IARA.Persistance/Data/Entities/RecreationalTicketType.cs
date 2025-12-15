using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

public partial class RecreationalTicketType
{
    [Key]
    public int TicketTypeId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string TypeName { get; set; } = null!;

    public int ValidityDays { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal PriceAdult { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal PriceUnder14 { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal PricePensioner { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal PriceDisabled { get; set; }

    [InverseProperty("TicketType")]
    public virtual ICollection<RecreationalTicket> RecreationalTickets { get; set; } = new List<RecreationalTicket>();
}
