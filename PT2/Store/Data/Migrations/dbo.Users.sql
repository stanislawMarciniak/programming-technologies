CREATE TABLE [dbo].[Users]
(
	[id] INT NOT NULL PRIMARY KEY,
	[nickname] VARCHAR(255) NOT NULL,
	[email] VARCHAR(255) NOT NULL,
	[balance] DECIMAL NOT NULL,
	[dateOfBirth] DATE NOT NULL
)