CREATE TABLE [dbo].[TransactionalOutbox](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[EventTypeName] [varchar](255) NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED
(
	[Id] ASC
))