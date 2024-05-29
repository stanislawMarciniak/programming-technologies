CREATE TABLE [dbo].[States]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [movieId] INT NOT NULL, 
    [movieQuantity] INT NOT NULL,

    CONSTRAINT [FK_States_Movies] FOREIGN KEY ([movieId]) REFERENCES [dbo].[Movies] ([id])
)