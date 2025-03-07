-- DefaultDbContext

CREATE TABLE [dbo].[person](
	[row] [int] IDENTITY(1,1) NOT NULL,
	[id] [char](36) NOT NULL,
	[name] [nvarchar](36) NOT NULL,
	[age] [int] NOT NULL,
	[birthday] [datetime] NOT NULL,
	[remark] [nvarchar](100) NULL,
 CONSTRAINT [pk__person] PRIMARY KEY CLUSTERED 
(
	[row] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [primary]
) ON [primary]

CREATE UNIQUE NONCLUSTERED INDEX [uni__person__id] ON [dbo].[person]
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [primary]
GO

CREATE NONCLUSTERED INDEX [idx__person__id] ON [dbo].[person]
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [primary]
GO

CREATE TABLE [dbo].[address](
	[row] [int] IDENTITY(1,1) NOT NULL,
	[id] [char](36) NOT NULL,
	[text] [nvarchar](100) NOT NULL,
 CONSTRAINT [pk__address] PRIMARY KEY CLUSTERED 
(
	[row] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [primary]
) ON [primary]
GO

ALTER TABLE [dbo].[address]  WITH CHECK ADD  CONSTRAINT [fk__address__id] FOREIGN KEY([id])
REFERENCES [dbo].[person] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[address] CHECK CONSTRAINT [fk__address__id]
GO

-- DataProtectionKeyDbContext

CREATE TABLE [dbo].[DataProtectionKeys](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FriendlyName] [nvarchar](max) NULL,
	[Xml] [nvarchar](max) NULL,
 CONSTRAINT [PK_DataProtectionKeys] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [primary]
) ON [PRIMARY] TEXTIMAGE_ON [primary]
GO

-- Cache

CREATE TABLE [dbo].[DataCache](
	[Id] [nvarchar](449) NOT NULL,
	[Value] [varbinary](max) NOT NULL,
	[ExpiresAtTime] [datetimeoffset](7) NOT NULL,
	[SlidingExpirationInSeconds] [bigint] NULL,
	[AbsoluteExpiration] [datetimeoffset](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
