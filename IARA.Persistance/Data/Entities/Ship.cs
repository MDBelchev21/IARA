using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("InternationalNumber", Name = "UQ__Ships__0D0EED16E42919A6", IsUnique = true)]
public partial class Ship
{
    [Key]
    public int ShipId { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? InternationalNumber { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? RadioCallSign { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ExternalMarking { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Name { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal Length { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal Width { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? GrossTonnage { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? Draft { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? MainEnginePower { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? FuelType { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? FuelCapacity { get; set; }

    public bool IsDeleted { get; set; }

    [InverseProperty("Ship")]
    public virtual ICollection<FishingPermit> FishingPermits { get; set; } = new List<FishingPermit>();

    [InverseProperty("Ship")]
    public virtual ICollection<FishingTrip> FishingTrips { get; set; } = new List<FishingTrip>();

    [InverseProperty("Ship")]
    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();

    [InverseProperty("Ship")]
    public virtual ICollection<ShipCrew> ShipCrews { get; set; } = new List<ShipCrew>();

    [InverseProperty("Ship")]
    public virtual ICollection<ShipEquipment> ShipEquipments { get; set; } = new List<ShipEquipment>();

    [InverseProperty("Ship")]
    public virtual ICollection<ShipOwner> ShipOwners { get; set; } = new List<ShipOwner>();
}
