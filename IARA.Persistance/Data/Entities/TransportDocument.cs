using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("TransportDate", Name = "IX_TransportDocuments_Date")]
[Index("DocumentNumber", Name = "UQ__Transpor__68993918964D757A", IsUnique = true)]
public partial class TransportDocument
{
    [Key]
    public int DocumentId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string DocumentNumber { get; set; } = null!;

    public DateTime TransportDate { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? OriginLocation { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string DestinationLocation { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string? VehicleRegistration { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? DriverName { get; set; }

    public DateTime? ReceivedOn { get; set; }

    [InverseProperty("TransportDocument")]
    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();

    [InverseProperty("Document")]
    public virtual ICollection<TransportLine> TransportLines { get; set; } = new List<TransportLine>();
}
