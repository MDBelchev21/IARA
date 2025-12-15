using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("CertificateNumber", Name = "UQ__Qualific__E384CE0F5CB2C613", IsUnique = true)]
public partial class Qualification
{
    [Key]
    public int QualificationId { get; set; }

    public int PersonId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string QualificationType { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string CertificateNumber { get; set; } = null!;

    public DateOnly IssuedOn { get; set; }

    public DateOnly? ValidUntil { get; set; }

    public bool IsRevoked { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("Qualifications")]
    public virtual Person Person { get; set; } = null!;

    [InverseProperty("Qualification")]
    public virtual ICollection<ShipCrew> ShipCrews { get; set; } = new List<ShipCrew>();
}
