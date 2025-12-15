using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IARA.Persistance.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LegalEntities",
                columns: table => new
                {
                    LegalEntityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    EIK = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: false),
                    Address = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LegalEnt__5266B1822B72A749", x => x.LegalEntityId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    EGN = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    IdNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Persons__AA2FFBE59EB41CA5", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "RecreationalTicketTypes",
                columns: table => new
                {
                    TicketTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ValidityDays = table.Column<int>(type: "int", nullable: false),
                    PriceAdult = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    PriceUnder14 = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    PricePensioner = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    PriceDisabled = table.Column<decimal>(type: "decimal(8,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Recreati__6CD68431C29FD8B9", x => x.TicketTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Ships",
                columns: table => new
                {
                    ShipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InternationalNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    RadioCallSign = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    ExternalMarking = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Length = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Width = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    GrossTonnage = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    Draft = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    MainEnginePower = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    FuelType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    FuelCapacity = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ships__2A05CAB39DECC879", x => x.ShipId);
                });

            migrationBuilder.CreateTable(
                name: "TransportDocuments",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    TransportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OriginLocation = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    DestinationLocation = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    VehicleRegistration = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    DriverName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ReceivedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Transpor__1ABEEF0FB08913D5", x => x.DocumentId);
                });

            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    AdministratorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Administ__ACDEFED3E993627C", x => x.AdministratorId);
                    table.ForeignKey(
                        name: "FK_Administrators_Persons",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inspectors",
                columns: table => new
                {
                    InspectorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    BadgeNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Inspecto__5FECC3DD213476CB", x => x.InspectorId);
                    table.ForeignKey(
                        name: "FK__Inspector__Perso__151B244E",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateTable(
                name: "Qualifications",
                columns: table => new
                {
                    QualificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    QualificationType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CertificateNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    IssuedOn = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidUntil = table.Column<DateOnly>(type: "date", nullable: true),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Qualific__C95C12AA660B733F", x => x.QualificationId);
                    table.ForeignKey(
                        name: "FK__Qualifica__Perso__4D94879B",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateTable(
                name: "RecreationalFishermen",
                columns: table => new
                {
                    RecFishermanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    TELKDecisionNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Recreati__0111CB387FF0FDCF", x => x.RecFishermanId);
                    table.ForeignKey(
                        name: "FK__Recreatio__Perso__04E4BC85",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateTable(
                name: "FishingPermits",
                columns: table => new
                {
                    PermitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermitNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ShipId = table.Column<int>(type: "int", nullable: false),
                    IssuedOn = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidUntil = table.Column<DateOnly>(type: "date", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__FishingP__0B0E6DD0AFB98956", x => x.PermitId);
                    table.ForeignKey(
                        name: "FK__FishingPe__ShipI__5DCAEF64",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "ShipId");
                });

            migrationBuilder.CreateTable(
                name: "ShipEquipment",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipId = table.Column<int>(type: "int", nullable: false),
                    EquipmentType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    EquipmentName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Length = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    MeshSize = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ShipEqui__34474479C6189FA8", x => x.EquipmentId);
                    table.ForeignKey(
                        name: "FK__ShipEquip__ShipI__5812160E",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "ShipId");
                });

            migrationBuilder.CreateTable(
                name: "ShipOwners",
                columns: table => new
                {
                    ShipOwnerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    LegalEntityId = table.Column<int>(type: "int", nullable: true),
                    OwnershipShare = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 100.00m),
                    ValidFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidTo = table.Column<DateOnly>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ShipOwne__6E9C97C445083BFE", x => x.ShipOwnerId);
                    table.ForeignKey(
                        name: "FK__ShipOwner__Legal__46E78A0C",
                        column: x => x.LegalEntityId,
                        principalTable: "LegalEntities",
                        principalColumn: "LegalEntityId");
                    table.ForeignKey(
                        name: "FK__ShipOwner__Perso__45F365D3",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                    table.ForeignKey(
                        name: "FK__ShipOwner__ShipI__44FF419A",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "ShipId");
                });

            migrationBuilder.CreateTable(
                name: "TransportLines",
                columns: table => new
                {
                    TransportLineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    BatchNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    SpeciesName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    WeightKg = table.Column<decimal>(type: "decimal(10,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Transpor__A5BE6656021E5FDD", x => x.TransportLineId);
                    table.ForeignKey(
                        name: "FK__Transport__Docum__7F2BE32F",
                        column: x => x.DocumentId,
                        principalTable: "TransportDocuments",
                        principalColumn: "DocumentId");
                });

            migrationBuilder.CreateTable(
                name: "ShipCrew",
                columns: table => new
                {
                    ShipCrewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    IsCaptain = table.Column<bool>(type: "bit", nullable: false),
                    QualificationId = table.Column<int>(type: "int", nullable: true),
                    AssignedOn = table.Column<DateOnly>(type: "date", nullable: false),
                    RelievedOn = table.Column<DateOnly>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ShipCrew__89EACE1708DA1A64", x => x.ShipCrewId);
                    table.ForeignKey(
                        name: "FK__ShipCrew__Person__52593CB8",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                    table.ForeignKey(
                        name: "FK__ShipCrew__Qualif__5441852A",
                        column: x => x.QualificationId,
                        principalTable: "Qualifications",
                        principalColumn: "QualificationId");
                    table.ForeignKey(
                        name: "FK__ShipCrew__ShipId__5165187F",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "ShipId");
                });

            migrationBuilder.CreateTable(
                name: "RecreationalTickets",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    RecFishermanId = table.Column<int>(type: "int", nullable: false),
                    TicketTypeId = table.Column<int>(type: "int", nullable: false),
                    IssuedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    PurchaseChannel = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    QRCode = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Recreati__712CC60726B34F4E", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK__Recreatio__RecFi__0A9D95DB",
                        column: x => x.RecFishermanId,
                        principalTable: "RecreationalFishermen",
                        principalColumn: "RecFishermanId");
                    table.ForeignKey(
                        name: "FK__Recreatio__Ticke__0B91BA14",
                        column: x => x.TicketTypeId,
                        principalTable: "RecreationalTicketTypes",
                        principalColumn: "TicketTypeId");
                });

            migrationBuilder.CreateTable(
                name: "FishingTrips",
                columns: table => new
                {
                    TripId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipId = table.Column<int>(type: "int", nullable: false),
                    PermitId = table.Column<int>(type: "int", nullable: false),
                    DepartureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeparturePort = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReturnPort = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    TripStatus = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "InProgress")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__FishingT__51DC713E021FF154", x => x.TripId);
                    table.ForeignKey(
                        name: "FK__FishingTr__Permi__68487DD7",
                        column: x => x.PermitId,
                        principalTable: "FishingPermits",
                        principalColumn: "PermitId");
                    table.ForeignKey(
                        name: "FK__FishingTr__ShipI__6754599E",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "ShipId");
                });

            migrationBuilder.CreateTable(
                name: "PermitEquipment",
                columns: table => new
                {
                    PermitEquipmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermitId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PermitEq__535AC3FEEFB6D78F", x => x.PermitEquipmentId);
                    table.ForeignKey(
                        name: "FK__PermitEqu__Equip__6477ECF3",
                        column: x => x.EquipmentId,
                        principalTable: "ShipEquipment",
                        principalColumn: "EquipmentId");
                    table.ForeignKey(
                        name: "FK__PermitEqu__Permi__6383C8BA",
                        column: x => x.PermitId,
                        principalTable: "FishingPermits",
                        principalColumn: "PermitId");
                });

            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    InspectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectorId = table.Column<int>(type: "int", nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InspectionType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ShipId = table.Column<int>(type: "int", nullable: true),
                    TransportDocumentId = table.Column<int>(type: "int", nullable: true),
                    RecTicketId = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    ViolationFound = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Inspecti__30B2DC083A8C0695", x => x.InspectionId);
                    table.ForeignKey(
                        name: "FK__Inspectio__Inspe__18EBB532",
                        column: x => x.InspectorId,
                        principalTable: "Inspectors",
                        principalColumn: "InspectorId");
                    table.ForeignKey(
                        name: "FK__Inspectio__RecTi__1BC821DD",
                        column: x => x.RecTicketId,
                        principalTable: "RecreationalTickets",
                        principalColumn: "TicketId");
                    table.ForeignKey(
                        name: "FK__Inspectio__ShipI__19DFD96B",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "ShipId");
                    table.ForeignKey(
                        name: "FK__Inspectio__Trans__1AD3FDA4",
                        column: x => x.TransportDocumentId,
                        principalTable: "TransportDocuments",
                        principalColumn: "DocumentId");
                });

            migrationBuilder.CreateTable(
                name: "RecreationalCatches",
                columns: table => new
                {
                    RecCatchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    SpeciesName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    CatchDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    WeightKg = table.Column<decimal>(type: "decimal(6,3)", nullable: true),
                    RegisteredVia = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Recreati__1D824C4D89A72E1F", x => x.RecCatchId);
                    table.ForeignKey(
                        name: "FK__Recreatio__Ticke__10566F31",
                        column: x => x.TicketId,
                        principalTable: "RecreationalTickets",
                        principalColumn: "TicketId");
                });

            migrationBuilder.CreateTable(
                name: "FishingOperations",
                columns: table => new
                {
                    OperationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Location = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    DurationHours = table.Column<decimal>(type: "decimal(6,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__FishingO__A4F5FC4454C8DD1E", x => x.OperationId);
                    table.ForeignKey(
                        name: "FK__FishingOp__Equip__6D0D32F4",
                        column: x => x.EquipmentId,
                        principalTable: "ShipEquipment",
                        principalColumn: "EquipmentId");
                    table.ForeignKey(
                        name: "FK__FishingOp__TripI__6C190EBB",
                        column: x => x.TripId,
                        principalTable: "FishingTrips",
                        principalColumn: "TripId");
                });

            migrationBuilder.CreateTable(
                name: "Landings",
                columns: table => new
                {
                    LandingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    LandingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Port = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    TotalWeight = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    ApprovedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Landings__F969CE7F8AA0BD78", x => x.LandingId);
                    table.ForeignKey(
                        name: "FK__Landings__TripId__73BA3083",
                        column: x => x.TripId,
                        principalTable: "FishingTrips",
                        principalColumn: "TripId");
                });

            migrationBuilder.CreateTable(
                name: "Violations",
                columns: table => new
                {
                    ViolationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionId = table.Column<int>(type: "int", nullable: false),
                    ViolationType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ViolatorPersonId = table.Column<int>(type: "int", nullable: true),
                    ViolatorLegalEntityId = table.Column<int>(type: "int", nullable: true),
                    ActNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    FineAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    FineStatus = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Violatio__18B6DC08FD99E4B1", x => x.ViolationId);
                    table.ForeignKey(
                        name: "FK__Violation__Inspe__208CD6FA",
                        column: x => x.InspectionId,
                        principalTable: "Inspections",
                        principalColumn: "InspectionId");
                    table.ForeignKey(
                        name: "FK__Violation__Viola__2180FB33",
                        column: x => x.ViolatorPersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                    table.ForeignKey(
                        name: "FK__Violation__Viola__22751F6C",
                        column: x => x.ViolatorLegalEntityId,
                        principalTable: "LegalEntities",
                        principalColumn: "LegalEntityId");
                });

            migrationBuilder.CreateTable(
                name: "Catches",
                columns: table => new
                {
                    CatchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationId = table.Column<int>(type: "int", nullable: false),
                    SpeciesName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    WeightKg = table.Column<decimal>(type: "decimal(10,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Catches__7418997E14C2FF80", x => x.CatchId);
                    table.ForeignKey(
                        name: "FK__Catches__Operati__6FE99F9F",
                        column: x => x.OperationId,
                        principalTable: "FishingOperations",
                        principalColumn: "OperationId");
                });

            migrationBuilder.CreateTable(
                name: "LandingLines",
                columns: table => new
                {
                    LandingLineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LandingId = table.Column<int>(type: "int", nullable: false),
                    CatchId = table.Column<int>(type: "int", nullable: true),
                    BatchNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    SpeciesName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    WeightKg = table.Column<decimal>(type: "decimal(10,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LandingL__9D6C96B34037F6DA", x => x.LandingLineId);
                    table.ForeignKey(
                        name: "FK__LandingLi__Catch__787EE5A0",
                        column: x => x.CatchId,
                        principalTable: "Catches",
                        principalColumn: "CatchId");
                    table.ForeignKey(
                        name: "FK__LandingLi__Landi__778AC167",
                        column: x => x.LandingId,
                        principalTable: "Landings",
                        principalColumn: "LandingId");
                });

            migrationBuilder.CreateIndex(
                name: "ix_Administrators_UserId",
                table: "Administrators",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "UQ__Administ__AA2FFBE4EEB9491C",
                table: "Administrators",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Catches_Operation",
                table: "Catches",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_FishingOperations_EquipmentId",
                table: "FishingOperations",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FishingOperations_Trip",
                table: "FishingOperations",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_FishingPermits_Ship",
                table: "FishingPermits",
                column: "ShipId",
                filter: "([IsRevoked]=(0))");

            migrationBuilder.CreateIndex(
                name: "IX_FishingPermits_ValidDates",
                table: "FishingPermits",
                columns: new[] { "ValidFrom", "ValidUntil" });

            migrationBuilder.CreateIndex(
                name: "UQ__FishingP__DA3C94EEAF67ECA5",
                table: "FishingPermits",
                column: "PermitNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FishingTrips_PermitId",
                table: "FishingTrips",
                column: "PermitId");

            migrationBuilder.CreateIndex(
                name: "IX_FishingTrips_Ship",
                table: "FishingTrips",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_Date",
                table: "Inspections",
                column: "InspectionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_Inspector",
                table: "Inspections",
                column: "InspectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_RecTicketId",
                table: "Inspections",
                column: "RecTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_ShipId",
                table: "Inspections",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_TransportDocumentId",
                table: "Inspections",
                column: "TransportDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspectors_PersonId",
                table: "Inspectors",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "UQ__Inspecto__D110FD567B78177B",
                table: "Inspectors",
                column: "BadgeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandingLines_Batch",
                table: "LandingLines",
                column: "BatchNumber");

            migrationBuilder.CreateIndex(
                name: "IX_LandingLines_CatchId",
                table: "LandingLines",
                column: "CatchId");

            migrationBuilder.CreateIndex(
                name: "IX_LandingLines_LandingId",
                table: "LandingLines",
                column: "LandingId");

            migrationBuilder.CreateIndex(
                name: "UQ__LandingL__F869ED6D49D901A9",
                table: "LandingLines",
                column: "BatchNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Landings_Trip",
                table: "Landings",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "UQ__LegalEnt__C1901701AE43820C",
                table: "LegalEntities",
                column: "EIK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermitEquipment_EquipmentId",
                table: "PermitEquipment",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "UK_PermitEquipment",
                table: "PermitEquipment",
                columns: new[] { "PermitId", "EquipmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_EGN",
                table: "Persons",
                column: "EGN",
                filter: "([EGN] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "UQ__Persons__C1902746EA65F342",
                table: "Persons",
                column: "EGN",
                unique: true,
                filter: "[EGN] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Qualifications_Person",
                table: "Qualifications",
                column: "PersonId",
                filter: "([IsRevoked]=(0))");

            migrationBuilder.CreateIndex(
                name: "UQ__Qualific__E384CE0F5CB2C613",
                table: "Qualifications",
                column: "CertificateNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecreationalCatches_Ticket",
                table: "RecreationalCatches",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_RecreationalFishermen_Person",
                table: "RecreationalFishermen",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_RecreationalTickets_QR",
                table: "RecreationalTickets",
                column: "QRCode",
                filter: "([QRCode] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_RecreationalTickets_RecFishermanId",
                table: "RecreationalTickets",
                column: "RecFishermanId");

            migrationBuilder.CreateIndex(
                name: "IX_RecreationalTickets_TicketTypeId",
                table: "RecreationalTickets",
                column: "TicketTypeId");

            migrationBuilder.CreateIndex(
                name: "UQ__Recreati__5B869AD9310C093F",
                table: "RecreationalTickets",
                column: "QRCode",
                unique: true,
                filter: "[QRCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__Recreati__CBED06DADBEBD853",
                table: "RecreationalTickets",
                column: "TicketNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShipCrew_PersonId",
                table: "ShipCrew",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipCrew_QualificationId",
                table: "ShipCrew",
                column: "QualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipCrew_Ship",
                table: "ShipCrew",
                column: "ShipId",
                filter: "([IsActive]=(1))");

            migrationBuilder.CreateIndex(
                name: "IX_ShipEquipment_Ship",
                table: "ShipEquipment",
                column: "ShipId",
                filter: "([IsActive]=(1))");

            migrationBuilder.CreateIndex(
                name: "IX_ShipOwners_LegalEntityId",
                table: "ShipOwners",
                column: "LegalEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipOwners_PersonId",
                table: "ShipOwners",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipOwners_Ship",
                table: "ShipOwners",
                column: "ShipId",
                filter: "([IsActive]=(1))");

            migrationBuilder.CreateIndex(
                name: "UQ__Ships__0D0EED16E42919A6",
                table: "Ships",
                column: "InternationalNumber",
                unique: true,
                filter: "[InternationalNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransportDocuments_Date",
                table: "TransportDocuments",
                column: "TransportDate");

            migrationBuilder.CreateIndex(
                name: "UQ__Transpor__68993918964D757A",
                table: "TransportDocuments",
                column: "DocumentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportLines_Batch",
                table: "TransportLines",
                column: "BatchNumber");

            migrationBuilder.CreateIndex(
                name: "IX_TransportLines_DocumentId",
                table: "TransportLines",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Violations_Inspection",
                table: "Violations",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Violations_ViolatorLegalEntityId",
                table: "Violations",
                column: "ViolatorLegalEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Violations_ViolatorPersonId",
                table: "Violations",
                column: "ViolatorPersonId");

            migrationBuilder.CreateIndex(
                name: "UQ__Violatio__F29FB4B699BB8E21",
                table: "Violations",
                column: "ActNumber",
                unique: true,
                filter: "[ActNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrators");

            migrationBuilder.DropTable(
                name: "LandingLines");

            migrationBuilder.DropTable(
                name: "PermitEquipment");

            migrationBuilder.DropTable(
                name: "RecreationalCatches");

            migrationBuilder.DropTable(
                name: "ShipCrew");

            migrationBuilder.DropTable(
                name: "ShipOwners");

            migrationBuilder.DropTable(
                name: "TransportLines");

            migrationBuilder.DropTable(
                name: "Violations");

            migrationBuilder.DropTable(
                name: "Catches");

            migrationBuilder.DropTable(
                name: "Landings");

            migrationBuilder.DropTable(
                name: "Qualifications");

            migrationBuilder.DropTable(
                name: "Inspections");

            migrationBuilder.DropTable(
                name: "LegalEntities");

            migrationBuilder.DropTable(
                name: "FishingOperations");

            migrationBuilder.DropTable(
                name: "Inspectors");

            migrationBuilder.DropTable(
                name: "RecreationalTickets");

            migrationBuilder.DropTable(
                name: "TransportDocuments");

            migrationBuilder.DropTable(
                name: "ShipEquipment");

            migrationBuilder.DropTable(
                name: "FishingTrips");

            migrationBuilder.DropTable(
                name: "RecreationalFishermen");

            migrationBuilder.DropTable(
                name: "RecreationalTicketTypes");

            migrationBuilder.DropTable(
                name: "FishingPermits");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Ships");
        }
    }
}
