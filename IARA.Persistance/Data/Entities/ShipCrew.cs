using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Table("ShipCrew")]
public partial class ShipCrew
{
    [Key]
    public int ShipCrewId { get; set; }

    public int ShipId { get; set; }

    public int PersonId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Position { get; set; } = null!;

    public bool IsCaptain { get; set; }

    public int? QualificationId { get; set; }

    public DateOnly AssignedOn { get; set; }

    public DateOnly? RelievedOn { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("ShipCrews")]
    public virtual Person Person { get; set; } = null!;

    [ForeignKey("QualificationId")]
    [InverseProperty("ShipCrews")]
    public virtual Qualification? Qualification { get; set; }

    [ForeignKey("ShipId")]
    [InverseProperty("ShipCrews")]
    public virtual Ship Ship { get; set; } = null!;
}
