CREATE TABLE [dbo].[Events]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [stateId] INT NOT NULL, 
    [userId] INT NOT NULL, 
    [occurrenceDate] DATE NOT NULL,
    [type] VARCHAR(50) NOT NULL,
    [quantity] INT NULL

    CONSTRAINT [FK_Events_States] FOREIGN KEY ([stateId]) REFERENCES [dbo].[States] ([id]),
    CONSTRAINT [FK_Events_Users] FOREIGN KEY ([userId]) REFERENCES [dbo].[Users] ([id])
)