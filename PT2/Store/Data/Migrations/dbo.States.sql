CREATE TABLE [dbo].[States]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [productId] INT NOT NULL, 
    [productQuantity] INT NOT NULL,

    CONSTRAINT [FK_States_Products] FOREIGN KEY ([productId]) REFERENCES [dbo].[Products] ([id])
)