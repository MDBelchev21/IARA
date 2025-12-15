using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

public partial class ShipOwner
{
    [Key]
    public int ShipOwnerId { get; set; }

    public int ShipId { get; set; }

    public int? PersonId { get; set; }

    public int? LegalEntityId { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal OwnershipShare { get; set; }

    public DateOnly ValidFrom { get; set; }

    public DateOnly? ValidTo { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey("LegalEntityId")]
    [InverseProperty("ShipOwners")]
    public virtual LegalEntity? LegalEntity { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("ShipOwners")]
    public virtual Person? Person { get; set; }

    [ForeignKey("ShipId")]
    [InverseProperty("ShipOwners")]
    public virtual Ship Ship { get; set; } = null!;
}
