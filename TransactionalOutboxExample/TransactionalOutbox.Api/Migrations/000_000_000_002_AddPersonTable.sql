CREATE TABLE [dbo].[Person](
	[Id] VARCHAR(26) NOT NULL,
	[Name] NVARCHAR(255) NOT NULL,
	[Age] INT NOT NULL,
	[Address] NVARCHAR(255) NOT NULL,
PRIMARY KEY CLUSTERED
(
	[Id] ASC
))