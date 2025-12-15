using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("InspectionId", Name = "IX_Violations_Inspection")]
[Index("ActNumber", Name = "UQ__Violatio__F29FB4B699BB8E21", IsUnique = true)]
public partial class Violation
{
    [Key]
    public int ViolationId { get; set; }

    public int InspectionId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string ViolationType { get; set; } = null!;

    [Unicode(false)]
    public string? Description { get; set; }

    public int? ViolatorPersonId { get; set; }

    public int? ViolatorLegalEntityId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ActNumber { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? FineAmount { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? FineStatus { get; set; }

    public DateTime? PaymentDate { get; set; }

    [ForeignKey("InspectionId")]
    [InverseProperty("Violations")]
    public virtual Inspection Inspection { get; set; } = null!;

    [ForeignKey("ViolatorLegalEntityId")]
    [InverseProperty("Violations")]
    public virtual LegalEntity? ViolatorLegalEntity { get; set; }

    [ForeignKey("ViolatorPersonId")]
    [InverseProperty("Violations")]
    public virtual Person? ViolatorPerson { get; set; }
}
