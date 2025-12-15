using System;
using System.Collections.Generic;
using IARA.Persistance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IARA.Persistance.Data;

public partial class IARADbContext : DbContext
{
    public IARADbContext()
    {
    }

    public IARADbContext(DbContextOptions<IARADbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<Catch> Catches { get; set; }

    public virtual DbSet<FishingOperation> FishingOperations { get; set; }

    public virtual DbSet<FishingPermit> FishingPermits { get; set; }

    public virtual DbSet<FishingTrip> FishingTrips { get; set; }

    public virtual DbSet<Inspection> Inspections { get; set; }

    public virtual DbSet<Inspector> Inspectors { get; set; }

    public virtual DbSet<Landing> Landings { get; set; }

    public virtual DbSet<LandingLine> LandingLines { get; set; }

    public virtual DbSet<LegalEntity> LegalEntities { get; set; }

    public virtual DbSet<PermitEquipment> PermitEquipments { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    public virtual DbSet<Qualification> Qualifications { get; set; }

    public virtual DbSet<RecreationalCatch> RecreationalCatches { get; set; }

    public virtual DbSet<RecreationalFisherman> RecreationalFishermen { get; set; }

    public virtual DbSet<RecreationalTicket> RecreationalTickets { get; set; }

    public virtual DbSet<RecreationalTicketType> RecreationalTicketTypes { get; set; }

    public virtual DbSet<Ship> Ships { get; set; }

    public virtual DbSet<ShipCrew> ShipCrews { get; set; }

    public virtual DbSet<ShipEquipment> ShipEquipments { get; set; }

    public virtual DbSet<ShipOwner> ShipOwners { get; set; }

    public virtual DbSet<TransportDocument> TransportDocuments { get; set; }

    public virtual DbSet<TransportLine> TransportLines { get; set; }

    public virtual DbSet<Violation> Violations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.AdministratorId).HasName("PK__Administ__ACDEFED3E993627C");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Person).WithOne(p => p.Administrator).HasConstraintName("FK_Administrators_Persons");
        });

        modelBuilder.Entity<Catch>(entity =>
        {
            entity.HasKey(e => e.CatchId).HasName("PK__Catches__7418997E14C2FF80");

            entity.HasOne(d => d.Operation).WithMany(p => p.Catches)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Catches__Operati__6FE99F9F");
        });

        modelBuilder.Entity<FishingOperation>(entity =>
        {
            entity.HasKey(e => e.OperationId).HasName("PK__FishingO__A4F5FC4454C8DD1E");

            entity.HasOne(d => d.Equipment).WithMany(p => p.FishingOperations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FishingOp__Equip__6D0D32F4");

            entity.HasOne(d => d.Trip).WithMany(p => p.FishingOperations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FishingOp__TripI__6C190EBB");
        });

        modelBuilder.Entity<FishingPermit>(entity =>
        {
            entity.HasKey(e => e.PermitId).HasName("PK__FishingP__0B0E6DD0AFB98956");

            entity.HasIndex(e => e.ShipId, "IX_FishingPermits_Ship").HasFilter("([IsRevoked]=(0))");

            entity.HasOne(d => d.Ship).WithMany(p => p.FishingPermits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FishingPe__ShipI__5DCAEF64");
        });

        modelBuilder.Entity<FishingTrip>(entity =>
        {
            entity.HasKey(e => e.TripId).HasName("PK__FishingT__51DC713E021FF154");

            entity.Property(e => e.TripStatus).HasDefaultValue("InProgress");

            entity.HasOne(d => d.Permit).WithMany(p => p.FishingTrips)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FishingTr__Permi__68487DD7");

            entity.HasOne(d => d.Ship).WithMany(p => p.FishingTrips)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FishingTr__ShipI__6754599E");
        });

        modelBuilder.Entity<Inspection>(entity =>
        {
            entity.HasKey(e => e.InspectionId).HasName("PK__Inspecti__30B2DC083A8C0695");

            entity.HasOne(d => d.Inspector).WithMany(p => p.Inspections)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inspectio__Inspe__18EBB532");

            entity.HasOne(d => d.RecTicket).WithMany(p => p.Inspections).HasConstraintName("FK__Inspectio__RecTi__1BC821DD");

            entity.HasOne(d => d.Ship).WithMany(p => p.Inspections).HasConstraintName("FK__Inspectio__ShipI__19DFD96B");

            entity.HasOne(d => d.TransportDocument).WithMany(p => p.Inspections).HasConstraintName("FK__Inspectio__Trans__1AD3FDA4");
        });

        modelBuilder.Entity<Inspector>(entity =>
        {
            entity.HasKey(e => e.InspectorId).HasName("PK__Inspecto__5FECC3DD213476CB");

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Person).WithMany(p => p.Inspectors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inspector__Perso__151B244E");
        });

        modelBuilder.Entity<Landing>(entity =>
        {
            entity.HasKey(e => e.LandingId).HasName("PK__Landings__F969CE7F8AA0BD78");

            entity.HasOne(d => d.Trip).WithMany(p => p.Landings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Landings__TripId__73BA3083");
        });

        modelBuilder.Entity<LandingLine>(entity =>
        {
            entity.HasKey(e => e.LandingLineId).HasName("PK__LandingL__9D6C96B34037F6DA");

            entity.HasOne(d => d.Catch).WithMany(p => p.LandingLines).HasConstraintName("FK__LandingLi__Catch__787EE5A0");

            entity.HasOne(d => d.Landing).WithMany(p => p.LandingLines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LandingLi__Landi__778AC167");
        });

        modelBuilder.Entity<LegalEntity>(entity =>
        {
            entity.HasKey(e => e.LegalEntityId).HasName("PK__LegalEnt__5266B1822B72A749");
        });

        modelBuilder.Entity<PermitEquipment>(entity =>
        {
            entity.HasKey(e => e.PermitEquipmentId).HasName("PK__PermitEq__535AC3FEEFB6D78F");

            entity.HasOne(d => d.Equipment).WithMany(p => p.PermitEquipments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PermitEqu__Equip__6477ECF3");

            entity.HasOne(d => d.Permit).WithMany(p => p.PermitEquipments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PermitEqu__Permi__6383C8BA");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK__Persons__AA2FFBE59EB41CA5");

            entity.HasIndex(e => e.EGN, "IX_Persons_EGN").HasFilter("([EGN] IS NOT NULL)");

            entity.Property(e => e.EGN).IsFixedLength();
        });

        modelBuilder.Entity<Qualification>(entity =>
        {
            entity.HasKey(e => e.QualificationId).HasName("PK__Qualific__C95C12AA660B733F");

            entity.HasIndex(e => e.PersonId, "IX_Qualifications_Person").HasFilter("([IsRevoked]=(0))");

            entity.HasOne(d => d.Person).WithMany(p => p.Qualifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Qualifica__Perso__4D94879B");
        });

        modelBuilder.Entity<RecreationalCatch>(entity =>
        {
            entity.HasKey(e => e.RecCatchId).HasName("PK__Recreati__1D824C4D89A72E1F");

            entity.HasOne(d => d.Ticket).WithMany(p => p.RecreationalCatches)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recreatio__Ticke__10566F31");
        });

        modelBuilder.Entity<RecreationalFisherman>(entity =>
        {
            entity.HasKey(e => e.RecFishermanId).HasName("PK__Recreati__0111CB387FF0FDCF");

            entity.HasOne(d => d.Person).WithMany(p => p.RecreationalFishermen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recreatio__Perso__04E4BC85");
        });

        modelBuilder.Entity<RecreationalTicket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Recreati__712CC60726B34F4E");

            entity.HasIndex(e => e.QRCode, "IX_RecreationalTickets_QR").HasFilter("([QRCode] IS NOT NULL)");

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.RecFisherman).WithMany(p => p.RecreationalTickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recreatio__RecFi__0A9D95DB");

            entity.HasOne(d => d.TicketType).WithMany(p => p.RecreationalTickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recreatio__Ticke__0B91BA14");
        });

        modelBuilder.Entity<RecreationalTicketType>(entity =>
        {
            entity.HasKey(e => e.TicketTypeId).HasName("PK__Recreati__6CD68431C29FD8B9");
        });

        modelBuilder.Entity<Ship>(entity =>
        {
            entity.HasKey(e => e.ShipId).HasName("PK__Ships__2A05CAB39DECC879");
        });

        modelBuilder.Entity<ShipCrew>(entity =>
        {
            entity.HasKey(e => e.ShipCrewId).HasName("PK__ShipCrew__89EACE1708DA1A64");

            entity.HasIndex(e => e.ShipId, "IX_ShipCrew_Ship").HasFilter("([IsActive]=(1))");

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Person).WithMany(p => p.ShipCrews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShipCrew__Person__52593CB8");

            entity.HasOne(d => d.Qualification).WithMany(p => p.ShipCrews).HasConstraintName("FK__ShipCrew__Qualif__5441852A");

            entity.HasOne(d => d.Ship).WithMany(p => p.ShipCrews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShipCrew__ShipId__5165187F");
        });

        modelBuilder.Entity<ShipEquipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId).HasName("PK__ShipEqui__34474479C6189FA8");

            entity.HasIndex(e => e.ShipId, "IX_ShipEquipment_Ship").HasFilter("([IsActive]=(1))");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.Ship).WithMany(p => p.ShipEquipments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShipEquip__ShipI__5812160E");
        });

        modelBuilder.Entity<ShipOwner>(entity =>
        {
            entity.HasKey(e => e.ShipOwnerId).HasName("PK__ShipOwne__6E9C97C445083BFE");

            entity.HasIndex(e => e.ShipId, "IX_ShipOwners_Ship").HasFilter("([IsActive]=(1))");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.OwnershipShare).HasDefaultValue(100.00m);

            entity.HasOne(d => d.LegalEntity).WithMany(p => p.ShipOwners).HasConstraintName("FK__ShipOwner__Legal__46E78A0C");

            entity.HasOne(d => d.Person).WithMany(p => p.ShipOwners).HasConstraintName("FK__ShipOwner__Perso__45F365D3");

            entity.HasOne(d => d.Ship).WithMany(p => p.ShipOwners)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShipOwner__ShipI__44FF419A");
        });

        modelBuilder.Entity<TransportDocument>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PK__Transpor__1ABEEF0FB08913D5");
        });

        modelBuilder.Entity<TransportLine>(entity =>
        {
            entity.HasKey(e => e.TransportLineId).HasName("PK__Transpor__A5BE6656021E5FDD");

            entity.HasOne(d => d.Document).WithMany(p => p.TransportLines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transport__Docum__7F2BE32F");
        });

        modelBuilder.Entity<Violation>(entity =>
        {
            entity.HasKey(e => e.ViolationId).HasName("PK__Violatio__18B6DC08FD99E4B1");

            entity.HasOne(d => d.Inspection).WithMany(p => p.Violations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Violation__Inspe__208CD6FA");

            entity.HasOne(d => d.ViolatorLegalEntity).WithMany(p => p.Violations).HasConstraintName("FK__Violation__Viola__22751F6C");

            entity.HasOne(d => d.ViolatorPerson).WithMany(p => p.Violations).HasConstraintName("FK__Violation__Viola__2180FB33");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
