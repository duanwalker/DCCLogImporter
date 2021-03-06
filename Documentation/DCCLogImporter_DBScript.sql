USE [bookvaultrpt]
GO

/****** Object:  Table [dbo].[ProcessLogFiles]    Script Date: 3/24/2017 10:29:30 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ProcessLogFiles](
	[RecordID] [bigint] IDENTITY(1,1) NOT NULL,
	[ProcessName] [varchar](50) NULL,
	[ProcessIndex] [numeric](18, 1) NULL,
	[ProcessLogFile] [varchar](200) NULL,
	[PublicationFileName] [varchar](200) NULL,
	[ProcessDate] [datetime] NULL,
	[SourceIP] [varchar](20) NULL,
	[ISBN] [varchar](20) NULL,
	[Exception] [varchar](max) NULL,
	[CreateDate] [datetime] NULL
)

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ProcessLogFiles] ADD  CONSTRAINT [DF__ProcessLo__Creat__6EF57B66]  DEFAULT (getutcdate()) FOR [CreateDate]
GO
