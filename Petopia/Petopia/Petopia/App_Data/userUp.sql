/*   woo-hoo!   */
/*-------------------------------------------------------------------------------------*/
CREATE TABLE [dbo].[PetopiaUsers] (
	[UserID] INT IDENTITY (1,1) NOT NULL, 
	/*------------------------------------*/
	[UserName] NVARCHAR(35) NULL,
	[Password] NVARCHAR(18) NULL,
	/*------------------------------------*/
	[FirstName] NVARCHAR(25) NOT NULL,
	[LastName] NVARCHAR(25) NOT NULL,
	/*------------------------------------*/
	[ASPNetIdentityID] NVARCHAR (128) NOT NULL,
	[IsOwner] BIT NOT NULL,
	[IsProvider] BIT NOT NULL,
	/*------------------------------------*/
	[MainPhone] NVARCHAR(12) NOT NULL,
	[AltPhone] NVARCHAR(12) NULL,
	[ResAddress01] NVARCHAR(50) NOT NULL,
	[ResAddress02] NVARCHAR(50),
	[ResCity] NVARCHAR(50) NOT NULL,
	[ResState] NVARCHAR(2) NOT NULL,
	[ResZipcode] NVARCHAR(5) NOT NULL,
	/*------------------------------------*/
	[ProfilePhoto] VARBINARY(MAX) NULL,
	/*------------------------------------*/
	/* these added later with alter table */
	[UserCaption] NVARCHAR(90) NULL,
	[GeneralLocation] NVARCHAR(90) NULL,
	[Tagline] NVARCHAR(90) NULL,
	[UserBio] NVARCHAR(max) NULL,
	/*---------------------------------*/

	CONSTRAINT[PK_dbo.PetopiaUsers] PRIMARY KEY CLUSTERED([UserID] ASC)
);
/*=====================================================================================*/
CREATE TABLE [dbo].[PetOwner] (
	[PetOwnerID] INT IDENTITY (1,1) NOT NULL,
	/*------------------------------------*/
	[AverageRating] INT(1),
	/*------------------------------------*/
	[GeneralNeeds] NVARCHAR(MAX) NOT NULL,
	[HomeAccess] NVARCHAR(MAX) NOT NULL,
	/*------------------------------------*/
	[UserID] INT NOT NULL,
	/*------------------------------------*/

	CONSTRAINT[PK_dbo.PetOwner] PRIMARY KEY CLUSTERED([PetOwnerID] ASC),

	CONSTRAINT[FK_dbo.PetopiaUsersOwner] FOREIGN KEY([UserID]) 
										 REFERENCES [dbo].[PetopiaUsers]([UserID])
);
/*=====================================================================================*/
CREATE TABLE [dbo].[Pet] (
    [PetID] INT IDENTITY (1,1) NOT NULL,
	/*------------------------------------*/
    [PetName] NVARCHAR(24) NOT NULL, 
    [Species] NVARCHAR(24) NOT NULL,
    [Breed] NVARCHAR(24) NULL,
    [Gender] NVARCHAR(18) NOT NULL,
	/* --- removed 'Altered?' ---*/
    [Birthdate] DATE NOT NULL,
    /* next two added later w/Alter Table */
    [PetCaption] NVARCHAR(90) NULL,
    [PetBio] NVARCHAR(MAX) NULL,
    /*------------------------------------*/
    [Weight] NVARCHAR(3) NULL,
    [HealthConcerns] NVARCHAR(MAX) NULL,
    [BehaviorConcerns] NVARCHAR(MAX) NULL,
    /* --- removed 'AccessInstructions' --- */
    [PetAccess] NVARCHAR(MAX) NULL,
    [NeedsDetails] NVARCHAR(MAX) NULL,
	/*------------------------------------*/
    [EmergencyContactName] NVARCHAR(45) NULL,
    [EmergencyContactPhone] NVARCHAR(12) NULL,
	/*------------------------------------*/
	[PetPhoto] VARBINARY(MAX) NULL,
	/*------------------------------------*/
    [PetOwnerID] INT NOT NULL,
	/*------------------------------------*/

    CONSTRAINT[PK_dbo.Pet] PRIMARY KEY CLUSTERED([PetID] ASC),

    CONSTRAINT[FK_dbo.PetOwner] FOREIGN KEY([PetOwnerID]) 
								REFERENCES [dbo].[PetOwner]([PetOwnerID])
);
/*=====================================================================================*/
CREATE TABLE [dbo].[CareProvider] (
	[CareProviderID] INT IDENTITY (1,1) NOT NULL,
	/*------------------------------------*/
	[AverageRating] INT(1),
	/*------------------------------------*/
	[ExperienceDetails] NVARCHAR(MAX) NOT NULL,
	/*------------------------------------*/
	[UserID] INT NOT NULL,
	/*------------------------------------*/

	CONSTRAINT[PK_dbo.CareProvider] PRIMARY KEY CLUSTERED([CareProviderID] ASC),

	CONSTRAINT[FK_dbo.PetopiaUsersProvider] FOREIGN KEY([UserID]) 
											REFERENCES [dbo].[PetopiaUsers]
);
/*=====================================================================================*/
CREATE TABLE [dbo].[UserBadge] (
	[UserBadgeID] INT IDENTITY (1,1) NOT NULL,

	[DogOwner] BIT NULL,
	[DogProvider] BIT NULL,

	[CatOwner] BIT NULL,
	[CatProvider] BIT NULL,

	[BirdOwner] BIT NULL,
	[BirdProvider] BIT NULL,

	[FishOwner] BIT NULL,
	[FishProvider] BIT NULL,

	[HorseOwner] BIT NULL,
	[HorseProvider] BIT NULL,

	[LivestockOwner] BIT NULL,
	[LivestockProvider] BIT NULL,

	[RabbitOwner] BIT NULL,
	[RabbitProvider] BIT NULL,

	[ReptileOwner] BIT NULL,
	[ReptileProvider] BIT NULL,

	[RodentOwner] BIT NULL,
	[RodentProvider] BIT NULL,

	[OtherOwner] BIT NULL,
	[OtherProvider] BIT NULL,
	/*---------------------------------*/
	[UserID] INT NOT NULL,
	/*---------------------------------*/

	CONSTRAINT[PK_dbo.UserBadge] PRIMARY KEY CLUSTERED([UserBadgeID] ASC),

	CONSTRAINT[FK_dbo.PetopiaUsersBadges] FOREIGN KEY([UserID]) 
										  REFERENCES [dbo].[PetopiaUsers]
);
/*=====================================================================================*/
/* who knew 'Transaction' was a reserved word?    haha!   =]                          */
/* i could make a table called that, but querying was a 'no' unless you did [ ]      */
/* so to save pain in the ass for queries i changed the name as you see here   =]   */
/* did ALTER TABLE to change 'TransactionDate' to 'StartDate'; add 'EndDate'       */
CREATE TABLE [dbo].[CareTransaction] (
	[TransactionID] INT IDENTITY (1,1) NOT NULL,
	/*------------------------------------*/
	[StartDate] DATE NOT NULL,
	[StartTime] TIME NOT NULL,
	/*------------------------------------*/
	[EndDate] DATE NOT NULL,
	[EndTime] TIME NOT NULL,
	/*------------------------------------*/
	/* added later w/Alter Table */
	[NeededThisVisit] NVARCXHAR(MAX) NOT NULL,
	/*------------------------------------*/
	[CareProvided] NVARCHAR(90) NULL,
	[CareReport] NVARCHAR(MAX) NULL,
	/*------------------------------------*/
	[Charge] DECIMAL(5,2) NULL,
	[Tip] DECIMAL(5,2) NULL,
	/*------------------------------------*/
	[PC_Rating] INT(1) NULL,
	[PC_Comments] NVARCHAR(MAX) NULL,
	[PO_Rating] INT(1) NULL,
	[PO_Comments] NVARCHAR(MAX) NULL,
	/*------------------------------------*/
	[Pending] BIT NULL,
	[Confirmed] BIT NULL,
	[Completed_PO] BIT NULL,
	[Completed_CP] BIT NULL,
	[IsPaid] BIT NULL,
	/*------------------------------------*/
	[PetOwnerID] INT NOT NULL,
	[PetID] INT NOT NULL,
	[CareProviderID] INT NOT NULL,
	/*------------------------------------*/

	CONSTRAINT[PK_dbo.Transaction] PRIMARY KEY CLUSTERED([TransactionID] ASC),

	CONSTRAINT[FK_dbo.Transaction_PetOwner] FOREIGN KEY([PetOwnerID])
											REFERENCES [dbo].[PetOwner],
	CONSTRAINT[FK_dbo.Transaction_Pet] FOREIGN KEY([PetID]) REFERENCES [dbo].[Pet],
	CONSTRAINT[FK_dbo.Transaction_CareProvider] FOREIGN KEY([CareProviderID]) 
												REFERENCES [dbo].[CareProvider]
);
/*=====================================================================================*/
/**/
/**/
/*=====================================================================================*/
/* from first go at everything -- here for posterity   =]   */
/*
CREATE TABLE [dbo].[Owner] (
	[OwnerID] NVARCHAR(120) NOT NULL,
	[Username] NVARCHAR(120),
	[FirstName] NVARCHAR(120) NOT NULL,
	[LastName] NVARCHAR(120) NOT NULL,
	CONSTRAINT[PK_dbo.Owner] PRIMARY KEY CLUSTERED([OwnerID] ASC)

);

CREATE TABLE [dbo].[Provider](
	[ProviderID] NVARCHAR(120) NOT NULL,
	[Username] NVARCHAR(120),
	[FirstName] NVARCHAR(120) NOT NULL,
	[LastName] NVARCHAR(120) NOT NULL,
	CONSTRAINT[PK_dbo.Provider] PRIMARY KEY CLUSTERED([ProviderID] ASC)

);
*/