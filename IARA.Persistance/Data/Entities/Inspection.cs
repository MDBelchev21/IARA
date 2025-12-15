using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("InspectionDate", Name = "IX_Inspections_Date")]
[Index("InspectorId", Name = "IX_Inspections_Inspector")]
public partial class Inspection
{
    [Key]
    public int InspectionId { get; set; }

    public int InspectorId { get; set; }

    public DateTime InspectionDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string InspectionType { get; set; } = null!;

    public int? ShipId { get; set; }

    public int? TransportDocumentId { get; set; }

    public int? RecTicketId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Location { get; set; }

    public bool ViolationFound { get; set; }

    [Unicode(false)]
    public string? Notes { get; set; }

    [ForeignKey("InspectorId")]
    [InverseProperty("Inspections")]
    public virtual Inspector Inspector { get; set; } = null!;

    [ForeignKey("RecTicketId")]
    [InverseProperty("Inspections")]
    public virtual RecreationalTicket? RecTicket { get; set; }

    [ForeignKey("ShipId")]
    [InverseProperty("Inspections")]
    public virtual Ship? Ship { get; set; }

    [ForeignKey("TransportDocumentId")]
    [InverseProperty("Inspections")]
    public virtual TransportDocument? TransportDocument { get; set; }

    [InverseProperty("Inspection")]
    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
