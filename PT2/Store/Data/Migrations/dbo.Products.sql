CREATE TABLE [dbo].[Movies]
(
	[id] INT NOT NULL PRIMARY KEY,
	[name] VARCHAR(255) NOT NULL,
	[price] FLOAT NOT NULL,
	[ageRestriction] INTEGER NOT NULL
)