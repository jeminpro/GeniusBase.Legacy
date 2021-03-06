USE [master]
GO
/****** Object:  Database [GeniusBase]    Script Date: 04/04/2018 05:33:18 ******/
CREATE DATABASE [GeniusBase]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'kbvault', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\kbvault.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'kbvault_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\kbvault_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [GeniusBase] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GeniusBase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [GeniusBase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GeniusBase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GeniusBase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GeniusBase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GeniusBase] SET ARITHABORT OFF 
GO
ALTER DATABASE [GeniusBase] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [GeniusBase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GeniusBase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GeniusBase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GeniusBase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GeniusBase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GeniusBase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GeniusBase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GeniusBase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GeniusBase] SET  DISABLE_BROKER 
GO
ALTER DATABASE [GeniusBase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GeniusBase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GeniusBase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GeniusBase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GeniusBase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GeniusBase] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [GeniusBase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GeniusBase] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [GeniusBase] SET  MULTI_USER 
GO
ALTER DATABASE [GeniusBase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GeniusBase] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GeniusBase] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GeniusBase] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [GeniusBase] SET DELAYED_DURABILITY = DISABLED 
GO
USE [GeniusBase]
GO
/****** Object:  Table [dbo].[Activities]    Script Date: 04/04/2018 05:33:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Activities](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ActivityDate] [datetime2](7) NOT NULL,
	[Operation] [nvarchar](50) NOT NULL,
	[Information] [nvarchar](500) NULL,
 CONSTRAINT [PK_Activities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Article]    Script Date: 04/04/2018 05:33:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Article](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](200) NOT NULL,
	[Content] [varchar](max) NULL,
	[PlainTextContent] [varchar](max) NULL,
	[Views] [int] NOT NULL CONSTRAINT [DF_Article_Views]  DEFAULT ((0)),
	[Likes] [int] NOT NULL CONSTRAINT [DF_Article_Likes]  DEFAULT ((0)),
	[Created] [datetime] NULL,
	[Edited] [datetime] NULL,
	[IsDraft] [int] NOT NULL CONSTRAINT [DF_Article_IsDraft]  DEFAULT ((1)),
	[PublishStartDate] [datetime] NULL,
	[PublishEndDate] [datetime] NULL,
	[Author] [bigint] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[SefName] [varchar](200) NOT NULL,
 CONSTRAINT [PK_Article] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_ARTICLE_SEFNAME] UNIQUE NONCLUSTERED 
(
	[SefName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ArticleTag]    Script Date: 04/04/2018 05:33:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArticleTag](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TagId] [int] NOT NULL,
	[ArticleId] [bigint] NOT NULL,
	[Author] [bigint] NOT NULL CONSTRAINT [DF_ArticleTag_Author]  DEFAULT ((1)),
 CONSTRAINT [PK_ArticleTag] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Attachment]    Script Date: 04/04/2018 05:33:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Attachment](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ArticleId] [bigint] NOT NULL,
	[Path] [nvarchar](2048) NOT NULL,
	[FileName] [nvarchar](2048) NOT NULL,
	[Extension] [nvarchar](5) NOT NULL,
	[Downloads] [int] NOT NULL,
	[Hash] [nvarchar](256) NULL,
	[HashTime] [datetime] NULL,
	[MimeType] [varchar](50) NULL,
	[Author] [bigint] NOT NULL,
 CONSTRAINT [PK_Attachment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Category]    Script Date: 04/04/2018 05:33:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[IsHot] [bit] NOT NULL CONSTRAINT [DF_Category_IsHot]  DEFAULT ((0)),
	[Parent] [int] NULL,
	[SefName] [varchar](200) NOT NULL,
	[Author] [bigint] NOT NULL CONSTRAINT [DF_Category_Author]  DEFAULT ((1)),
	[Icon] [nvarchar](200) NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_CATEGORY_SEFNAME] UNIQUE NONCLUSTERED 
(
	[SefName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[KbUser]    Script Date: 04/04/2018 05:33:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KbUser](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](500) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Email] [nvarchar](200) NULL,
	[Role] [nvarchar](50) NOT NULL,
	[Author] [bigint] NOT NULL CONSTRAINT [DF_KbUser_Author]  DEFAULT ((1)),
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Settings]    Script Date: 04/04/2018 05:33:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Settings](
	[CompanyName] [nvarchar](100) NOT NULL CONSTRAINT [DF_Table_1_ArticleCountPerCategoryOnHomePage]  DEFAULT ((5)),
	[TagLine] [nvarchar](500) NULL,
	[JumbotronText] [nvarchar](100) NULL,
	[ArticleCountPerCategoryOnHomePage] [smallint] NOT NULL CONSTRAINT [DF_Settings_ArticleCountPerCategoryOnHomePage]  DEFAULT ((5)),
	[ShareThisPublicKey] [nvarchar](50) NULL,
	[DisqusShortName] [nvarchar](150) NULL,
	[IndexFileExtensions] [varchar](2000) NULL,
	[ArticlePrefix] [nvarchar](50) NULL CONSTRAINT [DF_Settings_ArticlePrefix]  DEFAULT (N'KB'),
	[AnalyticsAccount] [nvarchar](50) NULL,
	[Author] [bigint] NOT NULL CONSTRAINT [DF_Settings_Author]  DEFAULT ((1)),
	[BackupPath] [nvarchar](2000) NULL,
	[ShowTotalArticleCountOnFrontPage] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[CompanyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Tag]    Script Date: 04/04/2018 05:33:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tag](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Author] [bigint] NOT NULL CONSTRAINT [DF_Tag_Author]  DEFAULT ((1)),
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  UserDefinedFunction [dbo].[SplitStrings_Moden]    Script Date: 04/04/2018 05:33:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SplitStrings_Moden]
(
   @List NVARCHAR(MAX),
   @Delimiter NVARCHAR(255)
)
RETURNS TABLE
WITH SCHEMABINDING AS
RETURN
  WITH E1(N)        AS ( SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 
                         UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 
                         UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1),
       E2(N)        AS (SELECT 1 FROM E1 a, E1 b),
       E4(N)        AS (SELECT 1 FROM E2 a, E2 b),
       E42(N)       AS (SELECT 1 FROM E4 a, E2 b),
       cteTally(N)  AS (SELECT 0 UNION ALL SELECT TOP (DATALENGTH(ISNULL(@List,1))) 
                         ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) FROM E42),
       cteStart(N1) AS (SELECT t.N+1 FROM cteTally t
                         WHERE (SUBSTRING(@List,t.N,1) = @Delimiter OR t.N = 0))
  SELECT Item = SUBSTRING(@List, s.N1, ISNULL(NULLIF(CHARINDEX(@Delimiter,@List,s.N1),0)-s.N1,8000))
    FROM cteStart s;



GO
/****** Object:  View [dbo].[VwArticleTagCount]    Script Date: 04/04/2018 05:33:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[VwArticleTagCount] as 
select top 20 count(*) as Amount,Name , t.Id from ArticleTag at left join tag t on at.TagId = t.Id 
	group  by t.name , t.Id	


GO
ALTER TABLE [dbo].[Attachment] ADD  CONSTRAINT [DF_Attachment_Downloads]  DEFAULT ((0)) FOR [Downloads]
GO
ALTER TABLE [dbo].[Attachment] ADD  CONSTRAINT [DF_Attachment_Author]  DEFAULT ((1)) FOR [Author]
GO
ALTER TABLE [dbo].[Activities]  WITH CHECK ADD  CONSTRAINT [FK_Activities_KbUser] FOREIGN KEY([UserId])
REFERENCES [dbo].[KbUser] ([Id])
GO
ALTER TABLE [dbo].[Activities] CHECK CONSTRAINT [FK_Activities_KbUser]
GO
ALTER TABLE [dbo].[Article]  WITH CHECK ADD  CONSTRAINT [FK_Article_Author_User] FOREIGN KEY([Author])
REFERENCES [dbo].[KbUser] ([Id])
GO
ALTER TABLE [dbo].[Article] CHECK CONSTRAINT [FK_Article_Author_User]
GO
ALTER TABLE [dbo].[Article]  WITH CHECK ADD  CONSTRAINT [FK_Article_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Article] CHECK CONSTRAINT [FK_Article_Category]
GO
ALTER TABLE [dbo].[ArticleTag]  WITH CHECK ADD  CONSTRAINT [FK_ArticleTag_Article] FOREIGN KEY([ArticleId])
REFERENCES [dbo].[Article] ([Id])
GO
ALTER TABLE [dbo].[ArticleTag] CHECK CONSTRAINT [FK_ArticleTag_Article]
GO
ALTER TABLE [dbo].[ArticleTag]  WITH CHECK ADD  CONSTRAINT [FK_ArticleTag_KbUser] FOREIGN KEY([Author])
REFERENCES [dbo].[KbUser] ([Id])
GO
ALTER TABLE [dbo].[ArticleTag] CHECK CONSTRAINT [FK_ArticleTag_KbUser]
GO
ALTER TABLE [dbo].[ArticleTag]  WITH CHECK ADD  CONSTRAINT [FK_ArticleTag_Tag] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tag] ([Id])
GO
ALTER TABLE [dbo].[ArticleTag] CHECK CONSTRAINT [FK_ArticleTag_Tag]
GO
ALTER TABLE [dbo].[Attachment]  WITH CHECK ADD  CONSTRAINT [FK_Attachment_Article] FOREIGN KEY([ArticleId])
REFERENCES [dbo].[Article] ([Id])
GO
ALTER TABLE [dbo].[Attachment] CHECK CONSTRAINT [FK_Attachment_Article]
GO
ALTER TABLE [dbo].[Attachment]  WITH CHECK ADD  CONSTRAINT [FK_Attachment_KbUser] FOREIGN KEY([Author])
REFERENCES [dbo].[KbUser] ([Id])
GO
ALTER TABLE [dbo].[Attachment] CHECK CONSTRAINT [FK_Attachment_KbUser]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_Category_KbUser] FOREIGN KEY([Author])
REFERENCES [dbo].[KbUser] ([Id])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_Category_KbUser]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_Category_Parent] FOREIGN KEY([Parent])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_Category_Parent]
GO
ALTER TABLE [dbo].[KbUser]  WITH CHECK ADD  CONSTRAINT [FK_KbUser_KbUser] FOREIGN KEY([Author])
REFERENCES [dbo].[KbUser] ([Id])
GO
ALTER TABLE [dbo].[KbUser] CHECK CONSTRAINT [FK_KbUser_KbUser]
GO
ALTER TABLE [dbo].[Settings]  WITH CHECK ADD  CONSTRAINT [FK_Settings_KbUser] FOREIGN KEY([Author])
REFERENCES [dbo].[KbUser] ([Id])
GO
ALTER TABLE [dbo].[Settings] CHECK CONSTRAINT [FK_Settings_KbUser]
GO
ALTER TABLE [dbo].[Tag]  WITH CHECK ADD  CONSTRAINT [FK_Tag_KbUser] FOREIGN KEY([Author])
REFERENCES [dbo].[KbUser] ([Id])
GO
ALTER TABLE [dbo].[Tag] CHECK CONSTRAINT [FK_Tag_KbUser]
GO
/****** Object:  StoredProcedure [dbo].[AssignTagsToArticle]    Script Date: 04/04/2018 05:33:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[AssignTagsToArticle]
	@ArticleId BIGINT,
	@Tags NVARCHAR(4000)
AS
BEGIN
	SET NOCOUNT ON
		
	declare TagList cursor for 
		Select Item From SplitStrings_Moden(@Tags,',')
	declare @NewTagId bigint
	declare @TagName  nvarchar(256)
	
	delete from ArticleTag where ArticleId = @ArticleId
	Open TagList
	Fetch Next From TagList Into @TagName
	While @@FETCH_STATUS = 0 
	BEGIN
		Select @NewTagId = Id From dbo.Tag Where Name = @TagName

		If @NewTagId Is Null 
		Begin
			Insert Into Tag(Name) Values(@TagName)
			Set @NewTagId = SCOPE_IDENTITY()
		End
		Insert Into ArticleTag(ArticleId,TagId) values(@ArticleId,@NewTagId)
		Fetch Next From TagList Into @TagName
		Set @NewTagId = Null
	END
	Close TagList
	Deallocate TagList
END




GO
/****** Object:  StoredProcedure [dbo].[DoSearch]    Script Date: 04/04/2018 05:33:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[DoSearch]( @term as nvarchar ) as
begin
select ArticleId,ArticleTitle from vw_SearchData 
where  ArticleContent like '%'+@term+'%' 
	Or ArticleTitle like '%'+@term+'%'
	Or TagName like '%'+@term+'%'
	Or CategoryName like '%'+@term+'%'
group by ArticleId,ArticleTitle,TagName+CategoryName+CAST(ArticleId as NVARCHAR) 

end


GO
/****** Object:  StoredProcedure [dbo].[GetSimilarArticles]    Script Date: 04/04/2018 05:33:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Version 0.31 --

Create PROCEDURE [dbo].[GetSimilarArticles]
	@ArticleId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select  Top 5	t.Id, t.SefName ,t.Title,t.PublishEndDate, t.PublishStartDate,t.IsDraft,
					sum(Relevance) as Relevance 
	From ( 
			select	ArticleId,
					TagId, 
					(
						Select Count(*) from ArticleTag i 
						Where i.ArticleId = @ArticleId and TagId = o.TagId
					) Relevance 
			from ArticleTag o 
			where ArticleId <>  @ArticleId
		  ) x 
	left join Article t on x.ArticleId = t.Id 
	group by t.Id, t.SefName ,t.Title,t.PublishEndDate, t.PublishStartDate,t.IsDraft
	having sum(Relevance) > 0 
	order by sum(Relevance) desc  

END

GO
/****** Object:  StoredProcedure [dbo].[GetTopTags]    Script Date: 04/04/2018 05:33:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetTopTags] As 
begin
	select top 20 amount*100/(Select sum(amount) from VwArticleTagCount) Ratio,Name,Id, 0 as FontSize from VwArticleTagCount
	order by amount*100/(Select sum(amount) from VwArticleTagCount) desc	
end


GO
/****** Object:  StoredProcedure [dbo].[RemoveTagFromArticles]    Script Date: 04/04/2018 05:33:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RemoveTagFromArticles]
	-- Add the parameters for the stored procedure here
	@TagId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	delete from ArticleTag where TagId = @TagId
END



GO
USE [master]
GO
ALTER DATABASE [GeniusBase] SET  READ_WRITE 
GO

SET IDENTITY_INSERT [dbo].[KbUser] ON 

GO
INSERT [dbo].[KbUser] ([Id], [UserName], [Password], [Name], [LastName], [Email], [Role]) VALUES (1, N'admin', N'euCzby8AnoczvmGxSOu7vhQw35kwMmY0NmIwZTYwM2I0ZWU2YTk0ZTZlMzRlZGJmY2Q2ZQ==', N'Admin', N'User', N'admin@GeniusBase.comx', N'Admin')
GO
SET IDENTITY_INSERT [dbo].[KbUser] OFF
GO
INSERT [dbo].[Settings] ([CompanyName], [TagLine], [JumbotronText], [ArticleCountPerCategoryOnHomePage], [ShareThisPublicKey], [DisqusShortName], [IndexFileExtensions]) VALUES (N'Default Company', N'Default company tag line', N'Default jumbatron', 10, NULL, NULL, N'pdf,docx,doc')
GO