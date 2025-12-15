using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("BadgeNumber", Name = "UQ__Inspecto__D110FD567B78177B", IsUnique = true)]
public partial class Inspector
{
    [Key]
    public int InspectorId { get; set; }

    public int PersonId { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string BadgeNumber { get; set; } = null!;

    public bool IsActive { get; set; }

    [InverseProperty("Inspector")]
    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();

    [ForeignKey("PersonId")]
    [InverseProperty("Inspectors")]
    public virtual Person Person { get; set; } = null!;
}
