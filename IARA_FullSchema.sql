IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [LegalEntities] (
    [LegalEntityId] int NOT NULL IDENTITY,
    [Name] varchar(200) NOT NULL,
    [EIK] varchar(13) NOT NULL,
    [Address] varchar(300) NULL,
    [Email] varchar(100) NULL,
    [Phone] varchar(20) NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK__LegalEnt__5266B1822B72A749] PRIMARY KEY ([LegalEntityId])
);

CREATE TABLE [Persons] (
    [PersonId] int NOT NULL IDENTITY,
    [FirstName] varchar(50) NOT NULL,
    [MiddleName] varchar(50) NULL,
    [LastName] varchar(50) NOT NULL,
    [EGN] char(10) NULL,
    [IdNumber] varchar(20) NULL,
    [DateOfBirth] date NULL,
    [Email] varchar(100) NULL,
    [Phone] varchar(20) NULL,
    [Address] varchar(300) NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK__Persons__AA2FFBE59EB41CA5] PRIMARY KEY ([PersonId])
);

CREATE TABLE [RecreationalTicketTypes] (
    [TicketTypeId] int NOT NULL IDENTITY,
    [TypeName] varchar(50) NOT NULL,
    [ValidityDays] int NOT NULL,
    [PriceAdult] decimal(8,2) NOT NULL,
    [PriceUnder14] decimal(8,2) NOT NULL,
    [PricePensioner] decimal(8,2) NOT NULL,
    [PriceDisabled] decimal(8,2) NOT NULL,
    CONSTRAINT [PK__Recreati__6CD68431C29FD8B9] PRIMARY KEY ([TicketTypeId])
);

CREATE TABLE [Ships] (
    [ShipId] int NOT NULL IDENTITY,
    [InternationalNumber] varchar(20) NULL,
    [RadioCallSign] varchar(20) NULL,
    [ExternalMarking] varchar(50) NOT NULL,
    [Name] varchar(100) NULL,
    [Length] decimal(6,2) NOT NULL,
    [Width] decimal(6,2) NOT NULL,
    [GrossTonnage] decimal(8,2) NULL,
    [Draft] decimal(6,2) NULL,
    [MainEnginePower] decimal(8,2) NULL,
    [FuelType] varchar(50) NULL,
    [FuelCapacity] decimal(8,2) NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK__Ships__2A05CAB39DECC879] PRIMARY KEY ([ShipId])
);

CREATE TABLE [TransportDocuments] (
    [DocumentId] int NOT NULL IDENTITY,
    [DocumentNumber] varchar(50) NOT NULL,
    [TransportDate] datetime2 NOT NULL,
    [OriginLocation] varchar(200) NULL,
    [DestinationLocation] varchar(200) NOT NULL,
    [VehicleRegistration] varchar(20) NULL,
    [DriverName] varchar(100) NULL,
    [ReceivedOn] datetime2 NULL,
    CONSTRAINT [PK__Transpor__1ABEEF0FB08913D5] PRIMARY KEY ([DocumentId])
);

CREATE TABLE [Administrators] (
    [AdministratorId] int NOT NULL IDENTITY,
    [PersonId] int NOT NULL,
    [DisplayName] varchar(100) NULL,
    [CreatedOn] datetime2 NOT NULL DEFAULT ((sysdatetime())),
    CONSTRAINT [PK__Administ__ACDEFED3E993627C] PRIMARY KEY ([AdministratorId]),
    CONSTRAINT [FK_Administrators_Persons] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([PersonId]) ON DELETE CASCADE
);

CREATE TABLE [Inspectors] (
    [InspectorId] int NOT NULL IDENTITY,
    [PersonId] int NOT NULL,
    [BadgeNumber] varchar(20) NOT NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK__Inspecto__5FECC3DD213476CB] PRIMARY KEY ([InspectorId]),
    CONSTRAINT [FK__Inspector__Perso__151B244E] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([PersonId])
);

CREATE TABLE [Qualifications] (
    [QualificationId] int NOT NULL IDENTITY,
    [PersonId] int NOT NULL,
    [QualificationType] varchar(50) NOT NULL,
    [CertificateNumber] varchar(50) NOT NULL,
    [IssuedOn] date NOT NULL,
    [ValidUntil] date NULL,
    [IsRevoked] bit NOT NULL,
    CONSTRAINT [PK__Qualific__C95C12AA660B733F] PRIMARY KEY ([QualificationId]),
    CONSTRAINT [FK__Qualifica__Perso__4D94879B] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([PersonId])
);

CREATE TABLE [RecreationalFishermen] (
    [RecFishermanId] int NOT NULL IDENTITY,
    [PersonId] int NOT NULL,
    [IsDisabled] bit NOT NULL,
    [TELKDecisionNumber] varchar(50) NULL,
    CONSTRAINT [PK__Recreati__0111CB387FF0FDCF] PRIMARY KEY ([RecFishermanId]),
    CONSTRAINT [FK__Recreatio__Perso__04E4BC85] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([PersonId])
);

CREATE TABLE [FishingPermits] (
    [PermitId] int NOT NULL IDENTITY,
    [PermitNumber] varchar(50) NOT NULL,
    [ShipId] int NOT NULL,
    [IssuedOn] date NOT NULL,
    [ValidFrom] date NOT NULL,
    [ValidUntil] date NOT NULL,
    [IsRevoked] bit NOT NULL,
    CONSTRAINT [PK__FishingP__0B0E6DD0AFB98956] PRIMARY KEY ([PermitId]),
    CONSTRAINT [FK__FishingPe__ShipI__5DCAEF64] FOREIGN KEY ([ShipId]) REFERENCES [Ships] ([ShipId])
);

CREATE TABLE [ShipEquipment] (
    [EquipmentId] int NOT NULL IDENTITY,
    [ShipId] int NOT NULL,
    [EquipmentType] varchar(50) NOT NULL,
    [EquipmentName] varchar(100) NULL,
    [Quantity] int NOT NULL DEFAULT 1,
    [Length] decimal(6,2) NULL,
    [MeshSize] decimal(6,2) NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK__ShipEqui__34474479C6189FA8] PRIMARY KEY ([EquipmentId]),
    CONSTRAINT [FK__ShipEquip__ShipI__5812160E] FOREIGN KEY ([ShipId]) REFERENCES [Ships] ([ShipId])
);

CREATE TABLE [ShipOwners] (
    [ShipOwnerId] int NOT NULL IDENTITY,
    [ShipId] int NOT NULL,
    [PersonId] int NULL,
    [LegalEntityId] int NULL,
    [OwnershipShare] decimal(5,2) NOT NULL DEFAULT 100.0,
    [ValidFrom] date NOT NULL,
    [ValidTo] date NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK__ShipOwne__6E9C97C445083BFE] PRIMARY KEY ([ShipOwnerId]),
    CONSTRAINT [FK__ShipOwner__Legal__46E78A0C] FOREIGN KEY ([LegalEntityId]) REFERENCES [LegalEntities] ([LegalEntityId]),
    CONSTRAINT [FK__ShipOwner__Perso__45F365D3] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([PersonId]),
    CONSTRAINT [FK__ShipOwner__ShipI__44FF419A] FOREIGN KEY ([ShipId]) REFERENCES [Ships] ([ShipId])
);

CREATE TABLE [TransportLines] (
    [TransportLineId] int NOT NULL IDENTITY,
    [DocumentId] int NOT NULL,
    [BatchNumber] varchar(50) NOT NULL,
    [SpeciesName] varchar(100) NOT NULL,
    [WeightKg] decimal(10,3) NOT NULL,
    CONSTRAINT [PK__Transpor__A5BE6656021E5FDD] PRIMARY KEY ([TransportLineId]),
    CONSTRAINT [FK__Transport__Docum__7F2BE32F] FOREIGN KEY ([DocumentId]) REFERENCES [TransportDocuments] ([DocumentId])
);

CREATE TABLE [ShipCrew] (
    [ShipCrewId] int NOT NULL IDENTITY,
    [ShipId] int NOT NULL,
    [PersonId] int NOT NULL,
    [Position] varchar(50) NOT NULL,
    [IsCaptain] bit NOT NULL,
    [QualificationId] int NULL,
    [AssignedOn] date NOT NULL,
    [RelievedOn] date NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK__ShipCrew__89EACE1708DA1A64] PRIMARY KEY ([ShipCrewId]),
    CONSTRAINT [FK__ShipCrew__Person__52593CB8] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([PersonId]),
    CONSTRAINT [FK__ShipCrew__Qualif__5441852A] FOREIGN KEY ([QualificationId]) REFERENCES [Qualifications] ([QualificationId]),
    CONSTRAINT [FK__ShipCrew__ShipId__5165187F] FOREIGN KEY ([ShipId]) REFERENCES [Ships] ([ShipId])
);

CREATE TABLE [RecreationalTickets] (
    [TicketId] int NOT NULL IDENTITY,
    [TicketNumber] varchar(50) NOT NULL,
    [RecFishermanId] int NOT NULL,
    [TicketTypeId] int NOT NULL,
    [IssuedOn] datetime2 NOT NULL,
    [ValidFrom] datetime2 NOT NULL,
    [ValidUntil] datetime2 NOT NULL,
    [Price] decimal(8,2) NOT NULL,
    [PurchaseChannel] varchar(20) NOT NULL,
    [QRCode] varchar(200) NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK__Recreati__712CC60726B34F4E] PRIMARY KEY ([TicketId]),
    CONSTRAINT [FK__Recreatio__RecFi__0A9D95DB] FOREIGN KEY ([RecFishermanId]) REFERENCES [RecreationalFishermen] ([RecFishermanId]),
    CONSTRAINT [FK__Recreatio__Ticke__0B91BA14] FOREIGN KEY ([TicketTypeId]) REFERENCES [RecreationalTicketTypes] ([TicketTypeId])
);

CREATE TABLE [FishingTrips] (
    [TripId] int NOT NULL IDENTITY,
    [ShipId] int NOT NULL,
    [PermitId] int NOT NULL,
    [DepartureDate] datetime2 NOT NULL,
    [DeparturePort] varchar(100) NULL,
    [ReturnDate] datetime2 NULL,
    [ReturnPort] varchar(100) NULL,
    [TripStatus] varchar(20) NOT NULL DEFAULT 'InProgress',
    CONSTRAINT [PK__FishingT__51DC713E021FF154] PRIMARY KEY ([TripId]),
    CONSTRAINT [FK__FishingTr__Permi__68487DD7] FOREIGN KEY ([PermitId]) REFERENCES [FishingPermits] ([PermitId]),
    CONSTRAINT [FK__FishingTr__ShipI__6754599E] FOREIGN KEY ([ShipId]) REFERENCES [Ships] ([ShipId])
);

CREATE TABLE [PermitEquipment] (
    [PermitEquipmentId] int NOT NULL IDENTITY,
    [PermitId] int NOT NULL,
    [EquipmentId] int NOT NULL,
    CONSTRAINT [PK__PermitEq__535AC3FEEFB6D78F] PRIMARY KEY ([PermitEquipmentId]),
    CONSTRAINT [FK__PermitEqu__Equip__6477ECF3] FOREIGN KEY ([EquipmentId]) REFERENCES [ShipEquipment] ([EquipmentId]),
    CONSTRAINT [FK__PermitEqu__Permi__6383C8BA] FOREIGN KEY ([PermitId]) REFERENCES [FishingPermits] ([PermitId])
);

CREATE TABLE [Inspections] (
    [InspectionId] int NOT NULL IDENTITY,
    [InspectorId] int NOT NULL,
    [InspectionDate] datetime2 NOT NULL,
    [InspectionType] varchar(50) NOT NULL,
    [ShipId] int NULL,
    [TransportDocumentId] int NULL,
    [RecTicketId] int NULL,
    [Location] varchar(200) NULL,
    [ViolationFound] bit NOT NULL,
    [Notes] varchar(max) NULL,
    CONSTRAINT [PK__Inspecti__30B2DC083A8C0695] PRIMARY KEY ([InspectionId]),
    CONSTRAINT [FK__Inspectio__Inspe__18EBB532] FOREIGN KEY ([InspectorId]) REFERENCES [Inspectors] ([InspectorId]),
    CONSTRAINT [FK__Inspectio__RecTi__1BC821DD] FOREIGN KEY ([RecTicketId]) REFERENCES [RecreationalTickets] ([TicketId]),
    CONSTRAINT [FK__Inspectio__ShipI__19DFD96B] FOREIGN KEY ([ShipId]) REFERENCES [Ships] ([ShipId]),
    CONSTRAINT [FK__Inspectio__Trans__1AD3FDA4] FOREIGN KEY ([TransportDocumentId]) REFERENCES [TransportDocuments] ([DocumentId])
);

CREATE TABLE [RecreationalCatches] (
    [RecCatchId] int NOT NULL IDENTITY,
    [TicketId] int NOT NULL,
    [SpeciesName] varchar(100) NOT NULL,
    [CatchDate] datetime2 NOT NULL,
    [Location] varchar(200) NULL,
    [Quantity] int NOT NULL,
    [WeightKg] decimal(6,3) NULL,
    [RegisteredVia] varchar(20) NOT NULL,
    CONSTRAINT [PK__Recreati__1D824C4D89A72E1F] PRIMARY KEY ([RecCatchId]),
    CONSTRAINT [FK__Recreatio__Ticke__10566F31] FOREIGN KEY ([TicketId]) REFERENCES [RecreationalTickets] ([TicketId])
);

CREATE TABLE [FishingOperations] (
    [OperationId] int NOT NULL IDENTITY,
    [TripId] int NOT NULL,
    [EquipmentId] int NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NULL,
    [Location] varchar(200) NULL,
    [Latitude] decimal(9,6) NULL,
    [Longitude] decimal(9,6) NULL,
    [DurationHours] decimal(6,2) NULL,
    CONSTRAINT [PK__FishingO__A4F5FC4454C8DD1E] PRIMARY KEY ([OperationId]),
    CONSTRAINT [FK__FishingOp__Equip__6D0D32F4] FOREIGN KEY ([EquipmentId]) REFERENCES [ShipEquipment] ([EquipmentId]),
    CONSTRAINT [FK__FishingOp__TripI__6C190EBB] FOREIGN KEY ([TripId]) REFERENCES [FishingTrips] ([TripId])
);

CREATE TABLE [Landings] (
    [LandingId] int NOT NULL IDENTITY,
    [TripId] int NOT NULL,
    [LandingDate] datetime2 NOT NULL,
    [Port] varchar(100) NOT NULL,
    [TotalWeight] decimal(10,3) NOT NULL,
    [ApprovedBy] int NULL,
    CONSTRAINT [PK__Landings__F969CE7F8AA0BD78] PRIMARY KEY ([LandingId]),
    CONSTRAINT [FK__Landings__TripId__73BA3083] FOREIGN KEY ([TripId]) REFERENCES [FishingTrips] ([TripId])
);

CREATE TABLE [Violations] (
    [ViolationId] int NOT NULL IDENTITY,
    [InspectionId] int NOT NULL,
    [ViolationType] varchar(100) NOT NULL,
    [Description] varchar(max) NULL,
    [ViolatorPersonId] int NULL,
    [ViolatorLegalEntityId] int NULL,
    [ActNumber] varchar(50) NULL,
    [FineAmount] decimal(10,2) NULL,
    [FineStatus] varchar(20) NULL,
    [PaymentDate] datetime2 NULL,
    CONSTRAINT [PK__Violatio__18B6DC08FD99E4B1] PRIMARY KEY ([ViolationId]),
    CONSTRAINT [FK__Violation__Inspe__208CD6FA] FOREIGN KEY ([InspectionId]) REFERENCES [Inspections] ([InspectionId]),
    CONSTRAINT [FK__Violation__Viola__2180FB33] FOREIGN KEY ([ViolatorPersonId]) REFERENCES [Persons] ([PersonId]),
    CONSTRAINT [FK__Violation__Viola__22751F6C] FOREIGN KEY ([ViolatorLegalEntityId]) REFERENCES [LegalEntities] ([LegalEntityId])
);

CREATE TABLE [Catches] (
    [CatchId] int NOT NULL IDENTITY,
    [OperationId] int NOT NULL,
    [SpeciesName] varchar(100) NOT NULL,
    [WeightKg] decimal(10,3) NOT NULL,
    CONSTRAINT [PK__Catches__7418997E14C2FF80] PRIMARY KEY ([CatchId]),
    CONSTRAINT [FK__Catches__Operati__6FE99F9F] FOREIGN KEY ([OperationId]) REFERENCES [FishingOperations] ([OperationId])
);

CREATE TABLE [LandingLines] (
    [LandingLineId] int NOT NULL IDENTITY,
    [LandingId] int NOT NULL,
    [CatchId] int NULL,
    [BatchNumber] varchar(50) NOT NULL,
    [SpeciesName] varchar(100) NOT NULL,
    [WeightKg] decimal(10,3) NOT NULL,
    CONSTRAINT [PK__LandingL__9D6C96B34037F6DA] PRIMARY KEY ([LandingLineId]),
    CONSTRAINT [FK__LandingLi__Catch__787EE5A0] FOREIGN KEY ([CatchId]) REFERENCES [Catches] ([CatchId]),
    CONSTRAINT [FK__LandingLi__Landi__778AC167] FOREIGN KEY ([LandingId]) REFERENCES [Landings] ([LandingId])
);

CREATE INDEX [ix_Administrators_UserId] ON [Administrators] ([PersonId]);

CREATE UNIQUE INDEX [UQ__Administ__AA2FFBE4EEB9491C] ON [Administrators] ([PersonId]);

CREATE INDEX [IX_Catches_Operation] ON [Catches] ([OperationId]);

CREATE INDEX [IX_FishingOperations_EquipmentId] ON [FishingOperations] ([EquipmentId]);

CREATE INDEX [IX_FishingOperations_Trip] ON [FishingOperations] ([TripId]);

CREATE INDEX [IX_FishingPermits_Ship] ON [FishingPermits] ([ShipId]) WHERE ([IsRevoked]=(0));

CREATE INDEX [IX_FishingPermits_ValidDates] ON [FishingPermits] ([ValidFrom], [ValidUntil]);

CREATE UNIQUE INDEX [UQ__FishingP__DA3C94EEAF67ECA5] ON [FishingPermits] ([PermitNumber]);

CREATE INDEX [IX_FishingTrips_PermitId] ON [FishingTrips] ([PermitId]);

CREATE INDEX [IX_FishingTrips_Ship] ON [FishingTrips] ([ShipId]);

CREATE INDEX [IX_Inspections_Date] ON [Inspections] ([InspectionDate]);

CREATE INDEX [IX_Inspections_Inspector] ON [Inspections] ([InspectorId]);

CREATE INDEX [IX_Inspections_RecTicketId] ON [Inspections] ([RecTicketId]);

CREATE INDEX [IX_Inspections_ShipId] ON [Inspections] ([ShipId]);

CREATE INDEX [IX_Inspections_TransportDocumentId] ON [Inspections] ([TransportDocumentId]);

CREATE INDEX [IX_Inspectors_PersonId] ON [Inspectors] ([PersonId]);

CREATE UNIQUE INDEX [UQ__Inspecto__D110FD567B78177B] ON [Inspectors] ([BadgeNumber]);

CREATE INDEX [IX_LandingLines_Batch] ON [LandingLines] ([BatchNumber]);

CREATE INDEX [IX_LandingLines_CatchId] ON [LandingLines] ([CatchId]);

CREATE INDEX [IX_LandingLines_LandingId] ON [LandingLines] ([LandingId]);

CREATE UNIQUE INDEX [UQ__LandingL__F869ED6D49D901A9] ON [LandingLines] ([BatchNumber]);

CREATE INDEX [IX_Landings_Trip] ON [Landings] ([TripId]);

CREATE UNIQUE INDEX [UQ__LegalEnt__C1901701AE43820C] ON [LegalEntities] ([EIK]);

CREATE INDEX [IX_PermitEquipment_EquipmentId] ON [PermitEquipment] ([EquipmentId]);

CREATE UNIQUE INDEX [UK_PermitEquipment] ON [PermitEquipment] ([PermitId], [EquipmentId]);

CREATE INDEX [IX_Persons_EGN] ON [Persons] ([EGN]) WHERE ([EGN] IS NOT NULL);

CREATE UNIQUE INDEX [UQ__Persons__C1902746EA65F342] ON [Persons] ([EGN]) WHERE [EGN] IS NOT NULL;

CREATE INDEX [IX_Qualifications_Person] ON [Qualifications] ([PersonId]) WHERE ([IsRevoked]=(0));

CREATE UNIQUE INDEX [UQ__Qualific__E384CE0F5CB2C613] ON [Qualifications] ([CertificateNumber]);

CREATE INDEX [IX_RecreationalCatches_Ticket] ON [RecreationalCatches] ([TicketId]);

CREATE INDEX [IX_RecreationalFishermen_Person] ON [RecreationalFishermen] ([PersonId]);

CREATE INDEX [IX_RecreationalTickets_QR] ON [RecreationalTickets] ([QRCode]) WHERE ([QRCode] IS NOT NULL);

CREATE INDEX [IX_RecreationalTickets_RecFishermanId] ON [RecreationalTickets] ([RecFishermanId]);

CREATE INDEX [IX_RecreationalTickets_TicketTypeId] ON [RecreationalTickets] ([TicketTypeId]);

CREATE UNIQUE INDEX [UQ__Recreati__5B869AD9310C093F] ON [RecreationalTickets] ([QRCode]) WHERE [QRCode] IS NOT NULL;

CREATE UNIQUE INDEX [UQ__Recreati__CBED06DADBEBD853] ON [RecreationalTickets] ([TicketNumber]);

CREATE INDEX [IX_ShipCrew_PersonId] ON [ShipCrew] ([PersonId]);

CREATE INDEX [IX_ShipCrew_QualificationId] ON [ShipCrew] ([QualificationId]);

CREATE INDEX [IX_ShipCrew_Ship] ON [ShipCrew] ([ShipId]) WHERE ([IsActive]=(1));

CREATE INDEX [IX_ShipEquipment_Ship] ON [ShipEquipment] ([ShipId]) WHERE ([IsActive]=(1));

CREATE INDEX [IX_ShipOwners_LegalEntityId] ON [ShipOwners] ([LegalEntityId]);

CREATE INDEX [IX_ShipOwners_PersonId] ON [ShipOwners] ([PersonId]);

CREATE INDEX [IX_ShipOwners_Ship] ON [ShipOwners] ([ShipId]) WHERE ([IsActive]=(1));

CREATE UNIQUE INDEX [UQ__Ships__0D0EED16E42919A6] ON [Ships] ([InternationalNumber]) WHERE [InternationalNumber] IS NOT NULL;

CREATE INDEX [IX_TransportDocuments_Date] ON [TransportDocuments] ([TransportDate]);

CREATE UNIQUE INDEX [UQ__Transpor__68993918964D757A] ON [TransportDocuments] ([DocumentNumber]);

CREATE INDEX [IX_TransportLines_Batch] ON [TransportLines] ([BatchNumber]);

CREATE INDEX [IX_TransportLines_DocumentId] ON [TransportLines] ([DocumentId]);

CREATE INDEX [IX_Violations_Inspection] ON [Violations] ([InspectionId]);

CREATE INDEX [IX_Violations_ViolatorLegalEntityId] ON [Violations] ([ViolatorLegalEntityId]);

CREATE INDEX [IX_Violations_ViolatorPersonId] ON [Violations] ([ViolatorPersonId]);

CREATE UNIQUE INDEX [UQ__Violatio__F29FB4B699BB8E21] ON [Violations] ([ActNumber]) WHERE [ActNumber] IS NOT NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251214212123_Initial', N'9.0.11');

ALTER TABLE [Persons] ADD [CreatedOn] datetime2 NULL;

ALTER TABLE [Persons] ADD [PasswordHash] varchar(500) NULL;

ALTER TABLE [Persons] ADD [RefreshToken] varchar(500) NULL;

ALTER TABLE [Persons] ADD [RefreshTokenExpiryTime] datetime2 NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251215184345_Add Authentication fields', N'9.0.11');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251215210001_Test', N'9.0.11');

COMMIT;
GO

