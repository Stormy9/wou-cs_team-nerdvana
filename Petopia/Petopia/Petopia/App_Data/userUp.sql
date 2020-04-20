/*   woo-hoo!   */
/*-------------------------------------------------------------------------------------*/
CREATE TABLE [dbo].[PetopiaUsers] (
	[UserID] INT IDENTITY (1,1) NOT NULL, 

	[UserName] NVARCHAR(120),
	[Password] NVARCHAR(50),
	[FirstName] NVARCHAR(50) NOT NULL,
	[LastName] NVARCHAR(50) NOT NULL,
	/* these added later with alter table */
	[UserCaption] NVARCHAR(72),
	[GeneralLocation] NVARCHAR(72),
	[Tagline] NVARCHAR(72),
	[UserBio] NVARCHAR(72),
	/*-----------------------------------*/
	[ASPNetIdentityID] NVARCHAR (128),
	[IsOwner] BIT NOT NULL,
	[IsProvider] BIT NOT NULL,
	[MainPhone] NVARCHAR(50) NOT NULL,
	[AltPhone] NVARCHAR(50),
	[ResAddress01] NVARCHAR(50) NOT NULL,
	[ResAddress02] NVARCHAR(50),
	[ResCity] NVARCHAR(50) NOT NULL,
	[ResState] NVARCHAR(50) NOT NULL,
	[ResZipcode] NVARCHAR(24) NOT NULL,
	[ProfilePhoto] VARBINARY(MAX),

	CONSTRAINT[PK_dbo.PetopiaUsers] PRIMARY KEY CLUSTERED([UserID] ASC)
);
/*-------------------------------------------------------------------------------------*/
CREATE TABLE [dbo].[PetOwner] (
	[PetOwnerID] INT IDENTITY (1,1) NOT NULL,

	[AverageRating] NVARCHAR(120),
	[GeneralNeeds] NVARCHAR(MAX) NOT NULL,
	[HomeAccess] NVARCHAR(MAX) NOT NULL,

	[UserID] INT,

	CONSTRAINT[PK_dbo.PetOwner] PRIMARY KEY CLUSTERED([PetOwnerID] ASC),

	CONSTRAINT[FK_dbo.PetopiaUsersOwner] FOREIGN KEY([UserID]) REFERENCES [dbo].[PetopiaUsers]([UserID])
);
/*-------------------------------------------------------------------------------------*/
CREATE TABLE [dbo].[Pet] (
	[PetID] INT IDENTITY (1,1) NOT NULL,

	[PetName] NVARCHAR(24) NOT NULL, 
	[Species] NVARCHAR(24) NOT NULL,
	[Breed] NVARCHAR(24),
	[Gender] NVARCHAR(8) NOT NULL,
	[Altered] NVARCHAR(8),
	[Birthdate] DATE,
	/* next two added later w/Alter Table */
	[PetCaption] NVARCHAR(90),
	[PetBio] NVARCHAR(MAX),
	/*------------------------------------*/
	[Weight] NVARCHAR(3),
	[HealthConcerns] NVARCHAR(MAX),
	[BehaviorConcerns] NVARCHAR(MAX),
	/* ---removed AccessInstructions--- */
	[PetAccess] NVARCHAR(MAX),
	[NeedsDetails] NVARCHAR(MAX),
	[EmergencyContactName] NVARCHAR(45),
	[EmergencyContactPhone] NVARCHAR(12),

	[PetOwnerID] INT,

	CONSTRAINT[PK_dbo.Pet] PRIMARY KEY CLUSTERED([PetID] ASC),

	CONSTRAINT[FK_dbo.PetOwner] FOREIGN KEY([PetOwnerID]) REFERENCES [dbo].[PetOwner]([PetOwnerID])
);
/*-------------------------------------------------------------------------------------*/
CREATE TABLE [dbo].[CareProvider] (
	[CareProviderID] INT IDENTITY (1,1) NOT NULL,

	[AverageRating] NVARCHAR(120),
	[ExperienceDetails] NVARCHAR(MAX) NOT NULL,

	[UserID] INT,

	CONSTRAINT[PK_dbo.CareProvider] PRIMARY KEY CLUSTERED([CareProviderID] ASC),

	CONSTRAINT[FK_dbo.PetopiaUsersProvider] FOREIGN KEY([UserID]) REFERENCES [dbo].[PetopiaUsers]
);

/*-------------------------------------------------------------------------------------*/
CREATE TABLE [dbo].[UserBadge] (
	[UserBadgeID] INT IDENTITY (1,1) NOT NULL,

	[DogOwner] BIT,
	[DogProvider] BIT,
	[CatOwner] BIT,
	[CatProvider] BIT,
	[BirdOwner] BIT,
	[BirdProvider] BIT,

	[UserID] INT,

	CONSTRAINT[PK_dbo.UserBadge] PRIMARY KEY CLUSTERED([UserBadgeID] ASC),

	CONSTRAINT[FK_dbo.PetopiaUsersBadges] FOREIGN KEY([UserID]) REFERENCES [dbo].[PetopiaUsers]
);

/*-------------------------------------------------------------------------------------*/
/* who knew 'Transaction' was a reserved word?                                        */
/* i could make a table called that, but querying was a 'no' unless you did [ ]      */
/* so to save pain in the ass for queries i changed the name as you see here   =]   */
/* did ALTER TABLE to change 'TransactionDate' to 'StartDate'; add 'EndDate'       */
CREATE TABLE [dbo].[CareTransaction] (
	[TransactionID] INT IDENTITY (1,1) NOT NULL,

	[StartDate] DATE NOT NULL,
	/* added later w/Alter Table */
	[EndDate] DATE NOT NULL,
	/*--------------------------*/
	[StartTime] TIME NOT NULL,
	[EndTime] TIME NOT NULL,
	/* added later w/Alter Table */
	[NeededThisVisit] NVARCXHAR(MAX),
	/*------------------------------*/
	[CareProvided] NVARCHAR(90),
	[CareReport] NVARCHAR(MAX),
	[Charge] MONEY,
	[Tip] MONEY,
	[PC_Rating] INT,
	[PC_Comments] NVARCHAR(MAX),
	[PO_Rating] INT,
	[PO_Comments] NVARCHAR(MAX),

	[PetOwnerID] INT NOT NULL,
	[CareProviderID] INT NOT NULL,
	[PetID] INT NOT NULL,

	CONSTRAINT[PK_dbo.Transaction] PRIMARY KEY CLUSTERED([TransactionID] ASC),

	CONSTRAINT[FK_dbo.Transaction_PetOwner] FOREIGN KEY([PetOwnerID])
											REFERENCES [dbo].[PetOwner],
	CONSTRAINT[FK_dbo.Transaction_CareProvider] FOREIGN KEY([CareProviderID]) 
												REFERENCES [dbo].[CareProvider],
	CONSTRAINT[FK_dbo.Transaction_Pet] FOREIGN KEY([PetID]) REFERENCES [dbo].[Pet]
);



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