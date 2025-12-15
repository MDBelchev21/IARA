using System;
using IARA.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IARA.Persistance.Migrations
{
    [DbContext(typeof(IARADbContext))]
    [Migration("20251215184345_Add Authentication fields")]
    partial class AddAuthenticationfields
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Administrator", b =>
                {
                    b.Property<int>("AdministratorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AdministratorId"));

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("(sysdatetime())");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("AdministratorId")
                        .HasName("PK__Administ__ACDEFED3E993627C");

                    b.HasIndex(new[] { "PersonId" }, "UQ__Administ__AA2FFBE4EEB9491C")
                        .IsUnique();

                    b.HasIndex(new[] { "PersonId" }, "ix_Administrators_UserId");

                    b.ToTable("Administrators");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Catch", b =>
                {
                    b.Property<int>("CatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CatchId"));

                    b.Property<int>("OperationId")
                        .HasColumnType("int");

                    b.Property<string>("SpeciesName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("WeightKg")
                        .HasColumnType("decimal(10, 3)");

                    b.HasKey("CatchId")
                        .HasName("PK__Catches__7418997E14C2FF80");

                    b.HasIndex(new[] { "OperationId" }, "IX_Catches_Operation");

                    b.ToTable("Catches");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.FishingOperation", b =>
                {
                    b.Property<int>("OperationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OperationId"));

                    b.Property<decimal?>("DurationHours")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("int");

                    b.Property<decimal?>("Latitude")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<string>("Location")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<decimal?>("Longitude")
                        .HasColumnType("decimal(9, 6)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TripId")
                        .HasColumnType("int");

                    b.HasKey("OperationId")
                        .HasName("PK__FishingO__A4F5FC4454C8DD1E");

                    b.HasIndex("EquipmentId");

                    b.HasIndex(new[] { "TripId" }, "IX_FishingOperations_Trip");

                    b.ToTable("FishingOperations");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.FishingPermit", b =>
                {
                    b.Property<int>("PermitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PermitId"));

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<DateOnly>("IssuedOn")
                        .HasColumnType("date");

                    b.Property<string>("PermitNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("ShipId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("ValidFrom")
                        .HasColumnType("date");

                    b.Property<DateOnly>("ValidUntil")
                        .HasColumnType("date");

                    b.HasKey("PermitId")
                        .HasName("PK__FishingP__0B0E6DD0AFB98956");

                    b.HasIndex(new[] { "ShipId" }, "IX_FishingPermits_Ship")
                        .HasFilter("([IsRevoked]=(0))");

                    b.HasIndex(new[] { "ValidFrom", "ValidUntil" }, "IX_FishingPermits_ValidDates");

                    b.HasIndex(new[] { "PermitNumber" }, "UQ__FishingP__DA3C94EEAF67ECA5")
                        .IsUnique();

                    b.ToTable("FishingPermits");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.FishingTrip", b =>
                {
                    b.Property<int>("TripId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TripId"));

                    b.Property<DateTime>("DepartureDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeparturePort")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("PermitId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReturnPort")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("ShipId")
                        .HasColumnType("int");

                    b.Property<string>("TripStatus")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasDefaultValue("InProgress");

                    b.HasKey("TripId")
                        .HasName("PK__FishingT__51DC713E021FF154");

                    b.HasIndex("PermitId");

                    b.HasIndex(new[] { "ShipId" }, "IX_FishingTrips_Ship");

                    b.ToTable("FishingTrips");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Inspection", b =>
                {
                    b.Property<int>("InspectionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InspectionId"));

                    b.Property<DateTime>("InspectionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("InspectionType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("InspectorId")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Notes")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<int?>("RecTicketId")
                        .HasColumnType("int");

                    b.Property<int?>("ShipId")
                        .HasColumnType("int");

                    b.Property<int?>("TransportDocumentId")
                        .HasColumnType("int");

                    b.Property<bool>("ViolationFound")
                        .HasColumnType("bit");

                    b.HasKey("InspectionId")
                        .HasName("PK__Inspecti__30B2DC083A8C0695");

                    b.HasIndex("RecTicketId");

                    b.HasIndex("ShipId");

                    b.HasIndex("TransportDocumentId");

                    b.HasIndex(new[] { "InspectionDate" }, "IX_Inspections_Date");

                    b.HasIndex(new[] { "InspectorId" }, "IX_Inspections_Inspector");

                    b.ToTable("Inspections");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Inspector", b =>
                {
                    b.Property<int>("InspectorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InspectorId"));

                    b.Property<string>("BadgeNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("InspectorId")
                        .HasName("PK__Inspecto__5FECC3DD213476CB");

                    b.HasIndex("PersonId");

                    b.HasIndex(new[] { "BadgeNumber" }, "UQ__Inspecto__D110FD567B78177B")
                        .IsUnique();

                    b.ToTable("Inspectors");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Landing", b =>
                {
                    b.Property<int>("LandingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LandingId"));

                    b.Property<int?>("ApprovedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("LandingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Port")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("TotalWeight")
                        .HasColumnType("decimal(10, 3)");

                    b.Property<int>("TripId")
                        .HasColumnType("int");

                    b.HasKey("LandingId")
                        .HasName("PK__Landings__F969CE7F8AA0BD78");

                    b.HasIndex(new[] { "TripId" }, "IX_Landings_Trip");

                    b.ToTable("Landings");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.LandingLine", b =>
                {
                    b.Property<int>("LandingLineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LandingLineId"));

                    b.Property<string>("BatchNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("CatchId")
                        .HasColumnType("int");

                    b.Property<int>("LandingId")
                        .HasColumnType("int");

                    b.Property<string>("SpeciesName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("WeightKg")
                        .HasColumnType("decimal(10, 3)");

                    b.HasKey("LandingLineId")
                        .HasName("PK__LandingL__9D6C96B34037F6DA");

                    b.HasIndex("CatchId");

                    b.HasIndex("LandingId");

                    b.HasIndex(new[] { "BatchNumber" }, "IX_LandingLines_Batch");

                    b.HasIndex(new[] { "BatchNumber" }, "UQ__LandingL__F869ED6D49D901A9")
                        .IsUnique();

                    b.ToTable("LandingLines");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.LegalEntity", b =>
                {
                    b.Property<int>("LegalEntityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LegalEntityId"));

                    b.Property<string>("Address")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)");

                    b.Property<string>("EIK")
                        .IsRequired()
                        .HasMaxLength(13)
                        .IsUnicode(false)
                        .HasColumnType("varchar(13)");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.HasKey("LegalEntityId")
                        .HasName("PK__LegalEnt__5266B1822B72A749");

                    b.HasIndex(new[] { "EIK" }, "UQ__LegalEnt__C1901701AE43820C")
                        .IsUnique();

                    b.ToTable("LegalEntities");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.PermitEquipment", b =>
                {
                    b.Property<int>("PermitEquipmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PermitEquipmentId"));

                    b.Property<int>("EquipmentId")
                        .HasColumnType("int");

                    b.Property<int>("PermitId")
                        .HasColumnType("int");

                    b.HasKey("PermitEquipmentId")
                        .HasName("PK__PermitEq__535AC3FEEFB6D78F");

                    b.HasIndex("EquipmentId");

                    b.HasIndex(new[] { "PermitId", "EquipmentId" }, "UK_PermitEquipment")
                        .IsUnique();

                    b.ToTable("PermitEquipment");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PersonId"));

                    b.Property<string>("Address")
                        .HasMaxLength(300)
                        .IsUnicode(false)
                        .HasColumnType("varchar(300)");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("EGN")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("char(10)")
                        .IsFixedLength();

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("IdNumber")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(500)
                        .IsUnicode(false)
                        .HasColumnType("varchar(500)");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.HasKey("PersonId")
                        .HasName("PK__Persons__AA2FFBE59EB41CA5");

                    b.HasIndex(new[] { "EGN" }, "IX_Persons_EGN")
                        .HasFilter("([EGN] IS NOT NULL)");

                    b.HasIndex(new[] { "EGN" }, "UQ__Persons__C1902746EA65F342")
                        .IsUnique()
                        .HasFilter("[EGN] IS NOT NULL");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Qualification", b =>
                {
                    b.Property<int>("QualificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QualificationId"));

                    b.Property<string>("CertificateNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<DateOnly>("IssuedOn")
                        .HasColumnType("date");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<string>("QualificationType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateOnly?>("ValidUntil")
                        .HasColumnType("date");

                    b.HasKey("QualificationId")
                        .HasName("PK__Qualific__C95C12AA660B733F");

                    b.HasIndex(new[] { "PersonId" }, "IX_Qualifications_Person")
                        .HasFilter("([IsRevoked]=(0))");

                    b.HasIndex(new[] { "CertificateNumber" }, "UQ__Qualific__E384CE0F5CB2C613")
                        .IsUnique();

                    b.ToTable("Qualifications");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.RecreationalCatch", b =>
                {
                    b.Property<int>("RecCatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RecCatchId"));

                    b.Property<DateTime>("CatchDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Location")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("RegisteredVia")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("SpeciesName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("TicketId")
                        .HasColumnType("int");

                    b.Property<decimal?>("WeightKg")
                        .HasColumnType("decimal(6, 3)");

                    b.HasKey("RecCatchId")
                        .HasName("PK__Recreati__1D824C4D89A72E1F");

                    b.HasIndex(new[] { "TicketId" }, "IX_RecreationalCatches_Ticket");

                    b.ToTable("RecreationalCatches");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.RecreationalFisherman", b =>
                {
                    b.Property<int>("RecFishermanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RecFishermanId"));

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("bit");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<string>("TELKDecisionNumber")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("RecFishermanId")
                        .HasName("PK__Recreati__0111CB387FF0FDCF");

                    b.HasIndex(new[] { "PersonId" }, "IX_RecreationalFishermen_Person");

                    b.ToTable("RecreationalFishermen");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.RecreationalTicket", b =>
                {
                    b.Property<int>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketId"));

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<DateTime>("IssuedOn")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<string>("PurchaseChannel")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("QRCode")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<int>("RecFishermanId")
                        .HasColumnType("int");

                    b.Property<string>("TicketNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("TicketTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ValidFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ValidUntil")
                        .HasColumnType("datetime2");

                    b.HasKey("TicketId")
                        .HasName("PK__Recreati__712CC60726B34F4E");

                    b.HasIndex("RecFishermanId");

                    b.HasIndex("TicketTypeId");

                    b.HasIndex(new[] { "QRCode" }, "IX_RecreationalTickets_QR")
                        .HasFilter("([QRCode] IS NOT NULL)");

                    b.HasIndex(new[] { "QRCode" }, "UQ__Recreati__5B869AD9310C093F")
                        .IsUnique()
                        .HasFilter("[QRCode] IS NOT NULL");

                    b.HasIndex(new[] { "TicketNumber" }, "UQ__Recreati__CBED06DADBEBD853")
                        .IsUnique();

                    b.ToTable("RecreationalTickets");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.RecreationalTicketType", b =>
                {
                    b.Property<int>("TicketTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketTypeId"));

                    b.Property<decimal>("PriceAdult")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<decimal>("PriceDisabled")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<decimal>("PricePensioner")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<decimal>("PriceUnder14")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("ValidityDays")
                        .HasColumnType("int");

                    b.HasKey("TicketTypeId")
                        .HasName("PK__Recreati__6CD68431C29FD8B9");

                    b.ToTable("RecreationalTicketTypes");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Ship", b =>
                {
                    b.Property<int>("ShipId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShipId"));

                    b.Property<decimal?>("Draft")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<string>("ExternalMarking")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal?>("FuelCapacity")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<string>("FuelType")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal?>("GrossTonnage")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<string>("InternationalNumber")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<decimal>("Length")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<decimal?>("MainEnginePower")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("RadioCallSign")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<decimal>("Width")
                        .HasColumnType("decimal(6, 2)");

                    b.HasKey("ShipId")
                        .HasName("PK__Ships__2A05CAB39DECC879");

                    b.HasIndex(new[] { "InternationalNumber" }, "UQ__Ships__0D0EED16E42919A6")
                        .IsUnique()
                        .HasFilter("[InternationalNumber] IS NOT NULL");

                    b.ToTable("Ships");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.ShipCrew", b =>
                {
                    b.Property<int>("ShipCrewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShipCrewId"));

                    b.Property<DateOnly>("AssignedOn")
                        .HasColumnType("date");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<bool>("IsCaptain")
                        .HasColumnType("bit");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("QualificationId")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("RelievedOn")
                        .HasColumnType("date");

                    b.Property<int>("ShipId")
                        .HasColumnType("int");

                    b.HasKey("ShipCrewId")
                        .HasName("PK__ShipCrew__89EACE1708DA1A64");

                    b.HasIndex("PersonId");

                    b.HasIndex("QualificationId");

                    b.HasIndex(new[] { "ShipId" }, "IX_ShipCrew_Ship")
                        .HasFilter("([IsActive]=(1))");

                    b.ToTable("ShipCrew");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.ShipEquipment", b =>
                {
                    b.Property<int>("EquipmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EquipmentId"));

                    b.Property<string>("EquipmentName")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("EquipmentType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<decimal?>("Length")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<decimal?>("MeshSize")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<int>("ShipId")
                        .HasColumnType("int");

                    b.HasKey("EquipmentId")
                        .HasName("PK__ShipEqui__34474479C6189FA8");

                    b.HasIndex(new[] { "ShipId" }, "IX_ShipEquipment_Ship")
                        .HasFilter("([IsActive]=(1))");

                    b.ToTable("ShipEquipment");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.ShipOwner", b =>
                {
                    b.Property<int>("ShipOwnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShipOwnerId"));

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int?>("LegalEntityId")
                        .HasColumnType("int");

                    b.Property<decimal>("OwnershipShare")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(5, 2)")
                        .HasDefaultValue(100.00m);

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.Property<int>("ShipId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("ValidFrom")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("ValidTo")
                        .HasColumnType("date");

                    b.HasKey("ShipOwnerId")
                        .HasName("PK__ShipOwne__6E9C97C445083BFE");

                    b.HasIndex("LegalEntityId");

                    b.HasIndex("PersonId");

                    b.HasIndex(new[] { "ShipId" }, "IX_ShipOwners_Ship")
                        .HasFilter("([IsActive]=(1))");

                    b.ToTable("ShipOwners");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.TransportDocument", b =>
                {
                    b.Property<int>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentId"));

                    b.Property<string>("DestinationLocation")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("DocumentNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("DriverName")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("OriginLocation")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime?>("ReceivedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TransportDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("VehicleRegistration")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.HasKey("DocumentId")
                        .HasName("PK__Transpor__1ABEEF0FB08913D5");

                    b.HasIndex(new[] { "TransportDate" }, "IX_TransportDocuments_Date");

                    b.HasIndex(new[] { "DocumentNumber" }, "UQ__Transpor__68993918964D757A")
                        .IsUnique();

                    b.ToTable("TransportDocuments");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.TransportLine", b =>
                {
                    b.Property<int>("TransportLineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransportLineId"));

                    b.Property<string>("BatchNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("DocumentId")
                        .HasColumnType("int");

                    b.Property<string>("SpeciesName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("WeightKg")
                        .HasColumnType("decimal(10, 3)");

                    b.HasKey("TransportLineId")
                        .HasName("PK__Transpor__A5BE6656021E5FDD");

                    b.HasIndex("DocumentId");

                    b.HasIndex(new[] { "BatchNumber" }, "IX_TransportLines_Batch");

                    b.ToTable("TransportLines");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Violation", b =>
                {
                    b.Property<int>("ViolationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ViolationId"));

                    b.Property<string>("ActNumber")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Description")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<decimal?>("FineAmount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("FineStatus")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<int>("InspectionId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ViolationType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("ViolatorLegalEntityId")
                        .HasColumnType("int");

                    b.Property<int?>("ViolatorPersonId")
                        .HasColumnType("int");

                    b.HasKey("ViolationId")
                        .HasName("PK__Violatio__18B6DC08FD99E4B1");

                    b.HasIndex("ViolatorLegalEntityId");

                    b.HasIndex("ViolatorPersonId");

                    b.HasIndex(new[] { "InspectionId" }, "IX_Violations_Inspection");

                    b.HasIndex(new[] { "ActNumber" }, "UQ__Violatio__F29FB4B699BB8E21")
                        .IsUnique()
                        .HasFilter("[ActNumber] IS NOT NULL");

                    b.ToTable("Violations");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Administrator", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.Person", "Person")
                        .WithOne("Administrator")
                        .HasForeignKey("IARA.Persistance.Data.Entities.Administrator", "PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Administrators_Persons");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Catch", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.FishingOperation", "Operation")
                        .WithMany("Catches")
                        .HasForeignKey("OperationId")
                        .IsRequired()
                        .HasConstraintName("FK__Catches__Operati__6FE99F9F");

                    b.Navigation("Operation");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.FishingOperation", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.ShipEquipment", "Equipment")
                        .WithMany("FishingOperations")
                        .HasForeignKey("EquipmentId")
                        .IsRequired()
                        .HasConstraintName("FK__FishingOp__Equip__6D0D32F4");

                    b.HasOne("IARA.Persistance.Data.Entities.FishingTrip", "Trip")
                        .WithMany("FishingOperations")
                        .HasForeignKey("TripId")
                        .IsRequired()
                        .HasConstraintName("FK__FishingOp__TripI__6C190EBB");

                    b.Navigation("Equipment");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.FishingPermit", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.Ship", "Ship")
                        .WithMany("FishingPermits")
                        .HasForeignKey("ShipId")
                        .IsRequired()
                        .HasConstraintName("FK__FishingPe__ShipI__5DCAEF64");

                    b.Navigation("Ship");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.FishingTrip", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.FishingPermit", "Permit")
                        .WithMany("FishingTrips")
                        .HasForeignKey("PermitId")
                        .IsRequired()
                        .HasConstraintName("FK__FishingTr__Permi__68487DD7");

                    b.HasOne("IARA.Persistance.Data.Entities.Ship", "Ship")
                        .WithMany("FishingTrips")
                        .HasForeignKey("ShipId")
                        .IsRequired()
                        .HasConstraintName("FK__FishingTr__ShipI__6754599E");

                    b.Navigation("Permit");

                    b.Navigation("Ship");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Inspection", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.Inspector", "Inspector")
                        .WithMany("Inspections")
                        .HasForeignKey("InspectorId")
                        .IsRequired()
                        .HasConstraintName("FK__Inspectio__Inspe__18EBB532");

                    b.HasOne("IARA.Persistance.Data.Entities.RecreationalTicket", "RecTicket")
                        .WithMany("Inspections")
                        .HasForeignKey("RecTicketId")
                        .HasConstraintName("FK__Inspectio__RecTi__1BC821DD");

                    b.HasOne("IARA.Persistance.Data.Entities.Ship", "Ship")
                        .WithMany("Inspections")
                        .HasForeignKey("ShipId")
                        .HasConstraintName("FK__Inspectio__ShipI__19DFD96B");

                    b.HasOne("IARA.Persistance.Data.Entities.TransportDocument", "TransportDocument")
                        .WithMany("Inspections")
                        .HasForeignKey("TransportDocumentId")
                        .HasConstraintName("FK__Inspectio__Trans__1AD3FDA4");

                    b.Navigation("Inspector");

                    b.Navigation("RecTicket");

                    b.Navigation("Ship");

                    b.Navigation("TransportDocument");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Inspector", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.Person", "Person")
                        .WithMany("Inspectors")
                        .HasForeignKey("PersonId")
                        .IsRequired()
                        .HasConstraintName("FK__Inspector__Perso__151B244E");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Landing", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.FishingTrip", "Trip")
                        .WithMany("Landings")
                        .HasForeignKey("TripId")
                        .IsRequired()
                        .HasConstraintName("FK__Landings__TripId__73BA3083");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.LandingLine", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.Catch", "Catch")
                        .WithMany("LandingLines")
                        .HasForeignKey("CatchId")
                        .HasConstraintName("FK__LandingLi__Catch__787EE5A0");

                    b.HasOne("IARA.Persistance.Data.Entities.Landing", "Landing")
                        .WithMany("LandingLines")
                        .HasForeignKey("LandingId")
                        .IsRequired()
                        .HasConstraintName("FK__LandingLi__Landi__778AC167");

                    b.Navigation("Catch");

                    b.Navigation("Landing");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.PermitEquipment", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.ShipEquipment", "Equipment")
                        .WithMany("PermitEquipments")
                        .HasForeignKey("EquipmentId")
                        .IsRequired()
                        .HasConstraintName("FK__PermitEqu__Equip__6477ECF3");

                    b.HasOne("IARA.Persistance.Data.Entities.FishingPermit", "Permit")
                        .WithMany("PermitEquipments")
                        .HasForeignKey("PermitId")
                        .IsRequired()
                        .HasConstraintName("FK__PermitEqu__Permi__6383C8BA");

                    b.Navigation("Equipment");

                    b.Navigation("Permit");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Qualification", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.Person", "Person")
                        .WithMany("Qualifications")
                        .HasForeignKey("PersonId")
                        .IsRequired()
                        .HasConstraintName("FK__Qualifica__Perso__4D94879B");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.RecreationalCatch", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.RecreationalTicket", "Ticket")
                        .WithMany("RecreationalCatches")
                        .HasForeignKey("TicketId")
                        .IsRequired()
                        .HasConstraintName("FK__Recreatio__Ticke__10566F31");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.RecreationalFisherman", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.Person", "Person")
                        .WithMany("RecreationalFishermen")
                        .HasForeignKey("PersonId")
                        .IsRequired()
                        .HasConstraintName("FK__Recreatio__Perso__04E4BC85");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.RecreationalTicket", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.RecreationalFisherman", "RecFisherman")
                        .WithMany("RecreationalTickets")
                        .HasForeignKey("RecFishermanId")
                        .IsRequired()
                        .HasConstraintName("FK__Recreatio__RecFi__0A9D95DB");

                    b.HasOne("IARA.Persistance.Data.Entities.RecreationalTicketType", "TicketType")
                        .WithMany("RecreationalTickets")
                        .HasForeignKey("TicketTypeId")
                        .IsRequired()
                        .HasConstraintName("FK__Recreatio__Ticke__0B91BA14");

                    b.Navigation("RecFisherman");

                    b.Navigation("TicketType");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.ShipCrew", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.Person", "Person")
                        .WithMany("ShipCrews")
                        .HasForeignKey("PersonId")
                        .IsRequired()
                        .HasConstraintName("FK__ShipCrew__Person__52593CB8");

                    b.HasOne("IARA.Persistance.Data.Entities.Qualification", "Qualification")
                        .WithMany("ShipCrews")
                        .HasForeignKey("QualificationId")
                        .HasConstraintName("FK__ShipCrew__Qualif__5441852A");

                    b.HasOne("IARA.Persistance.Data.Entities.Ship", "Ship")
                        .WithMany("ShipCrews")
                        .HasForeignKey("ShipId")
                        .IsRequired()
                        .HasConstraintName("FK__ShipCrew__ShipId__5165187F");

                    b.Navigation("Person");

                    b.Navigation("Qualification");

                    b.Navigation("Ship");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.ShipEquipment", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.Ship", "Ship")
                        .WithMany("ShipEquipments")
                        .HasForeignKey("ShipId")
                        .IsRequired()
                        .HasConstraintName("FK__ShipEquip__ShipI__5812160E");

                    b.Navigation("Ship");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.ShipOwner", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.LegalEntity", "LegalEntity")
                        .WithMany("ShipOwners")
                        .HasForeignKey("LegalEntityId")
                        .HasConstraintName("FK__ShipOwner__Legal__46E78A0C");

                    b.HasOne("IARA.Persistance.Data.Entities.Person", "Person")
                        .WithMany("ShipOwners")
                        .HasForeignKey("PersonId")
                        .HasConstraintName("FK__ShipOwner__Perso__45F365D3");

                    b.HasOne("IARA.Persistance.Data.Entities.Ship", "Ship")
                        .WithMany("ShipOwners")
                        .HasForeignKey("ShipId")
                        .IsRequired()
                        .HasConstraintName("FK__ShipOwner__ShipI__44FF419A");

                    b.Navigation("LegalEntity");

                    b.Navigation("Person");

                    b.Navigation("Ship");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.TransportLine", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.TransportDocument", "Document")
                        .WithMany("TransportLines")
                        .HasForeignKey("DocumentId")
                        .IsRequired()
                        .HasConstraintName("FK__Transport__Docum__7F2BE32F");

                    b.Navigation("Document");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Violation", b =>
                {
                    b.HasOne("IARA.Persistance.Data.Entities.Inspection", "Inspection")
                        .WithMany("Violations")
                        .HasForeignKey("InspectionId")
                        .IsRequired()
                        .HasConstraintName("FK__Violation__Inspe__208CD6FA");

                    b.HasOne("IARA.Persistance.Data.Entities.LegalEntity", "ViolatorLegalEntity")
                        .WithMany("Violations")
                        .HasForeignKey("ViolatorLegalEntityId")
                        .HasConstraintName("FK__Violation__Viola__22751F6C");

                    b.HasOne("IARA.Persistance.Data.Entities.Person", "ViolatorPerson")
                        .WithMany("Violations")
                        .HasForeignKey("ViolatorPersonId")
                        .HasConstraintName("FK__Violation__Viola__2180FB33");

                    b.Navigation("Inspection");

                    b.Navigation("ViolatorLegalEntity");

                    b.Navigation("ViolatorPerson");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Catch", b =>
                {
                    b.Navigation("LandingLines");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.FishingOperation", b =>
                {
                    b.Navigation("Catches");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.FishingPermit", b =>
                {
                    b.Navigation("FishingTrips");

                    b.Navigation("PermitEquipments");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.FishingTrip", b =>
                {
                    b.Navigation("FishingOperations");

                    b.Navigation("Landings");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Inspection", b =>
                {
                    b.Navigation("Violations");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Inspector", b =>
                {
                    b.Navigation("Inspections");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Landing", b =>
                {
                    b.Navigation("LandingLines");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.LegalEntity", b =>
                {
                    b.Navigation("ShipOwners");

                    b.Navigation("Violations");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Person", b =>
                {
                    b.Navigation("Administrator");

                    b.Navigation("Inspectors");

                    b.Navigation("Qualifications");

                    b.Navigation("RecreationalFishermen");

                    b.Navigation("ShipCrews");

                    b.Navigation("ShipOwners");

                    b.Navigation("Violations");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Qualification", b =>
                {
                    b.Navigation("ShipCrews");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.RecreationalFisherman", b =>
                {
                    b.Navigation("RecreationalTickets");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.RecreationalTicket", b =>
                {
                    b.Navigation("Inspections");

                    b.Navigation("RecreationalCatches");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.RecreationalTicketType", b =>
                {
                    b.Navigation("RecreationalTickets");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.Ship", b =>
                {
                    b.Navigation("FishingPermits");

                    b.Navigation("FishingTrips");

                    b.Navigation("Inspections");

                    b.Navigation("ShipCrews");

                    b.Navigation("ShipEquipments");

                    b.Navigation("ShipOwners");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.ShipEquipment", b =>
                {
                    b.Navigation("FishingOperations");

                    b.Navigation("PermitEquipments");
                });

            modelBuilder.Entity("IARA.Persistance.Data.Entities.TransportDocument", b =>
                {
                    b.Navigation("Inspections");

                    b.Navigation("TransportLines");
                });
#pragma warning restore 612, 618
        }
    }
}
