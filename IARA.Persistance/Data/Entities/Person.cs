using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("EGN", Name = "UQ__Persons__C1902746EA65F342", IsUnique = true)]
public partial class Person
{
    [Key]
    public int PersonId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? MiddleName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string? EGN { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? IdNumber { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? Address { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? PasswordHash { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool IsDeleted { get; set; }

    [InverseProperty("Person")]
    public virtual Administrator? Administrator { get; set; }

    [InverseProperty("Person")]
    public virtual ICollection<Inspector> Inspectors { get; set; } = new List<Inspector>();

    [InverseProperty("Person")]
    public virtual ICollection<Qualification> Qualifications { get; set; } = new List<Qualification>();

    [InverseProperty("Person")]
    public virtual ICollection<RecreationalFisherman> RecreationalFishermen { get; set; } = new List<RecreationalFisherman>();

    [InverseProperty("Person")]
    public virtual ICollection<ShipCrew> ShipCrews { get; set; } = new List<ShipCrew>();

    [InverseProperty("Person")]
    public virtual ICollection<ShipOwner> ShipOwners { get; set; } = new List<ShipOwner>();

    [InverseProperty("ViolatorPerson")]
    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
