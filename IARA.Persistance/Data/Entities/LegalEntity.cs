using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("EIK", Name = "UQ__LegalEnt__C1901701AE43820C", IsUnique = true)]
public partial class LegalEntity
{
    [Key]
    public int LegalEntityId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(13)]
    [Unicode(false)]
    public string EIK { get; set; } = null!;

    [StringLength(300)]
    [Unicode(false)]
    public string? Address { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Phone { get; set; }

    public bool IsDeleted { get; set; }

    [InverseProperty("LegalEntity")]
    public virtual ICollection<ShipOwner> ShipOwners { get; set; } = new List<ShipOwner>();

    [InverseProperty("ViolatorLegalEntity")]
    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
