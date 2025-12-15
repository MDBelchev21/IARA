using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("PersonId", Name = "IX_RecreationalFishermen_Person")]
public partial class RecreationalFisherman
{
    [Key]
    public int RecFishermanId { get; set; }

    public int PersonId { get; set; }

    public bool IsDisabled { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TELKDecisionNumber { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("RecreationalFishermen")]
    public virtual Person Person { get; set; } = null!;

    [InverseProperty("RecFisherman")]
    public virtual ICollection<RecreationalTicket> RecreationalTickets { get; set; } = new List<RecreationalTicket>();
}
