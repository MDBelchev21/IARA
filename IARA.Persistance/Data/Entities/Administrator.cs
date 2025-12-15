using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data.Entities;

[Index("PersonId", Name = "UQ__Administ__AA2FFBE4EEB9491C", IsUnique = true)]
[Index("PersonId", Name = "ix_Administrators_UserId")]
public partial class Administrator
{
    [Key]
    public int AdministratorId { get; set; }

    public int PersonId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? DisplayName { get; set; }

    public DateTime CreatedOn { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("Administrator")]
    public virtual Person Person { get; set; } = null!;
}
