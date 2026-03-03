
GO
/****** Object:  Table [dbo].[tbl_sslink]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_sslink](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[role] [nvarchar](50) NULL,
	[link] [nvarchar](50) NULL,
 CONSTRAINT [PK_tbl_sslink] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_sslogin]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_sslogin](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[emp_id] [nvarchar](50) NULL,
	[password] [nvarchar](50) NULL,
 CONSTRAINT [PK_tbl_sslogin] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ssnotification]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ssnotification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[emp_id] [nvarchar](50) NULL,
	[message] [nvarchar](500) NULL,
	[isread] [bit] NULL,
	[notify_on] [datetime] NULL,
 CONSTRAINT [PK_tbl_ssnotification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_sssublink]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_sssublink](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[link_id] [int] NULL,
	[sub_link] [nvarchar](50) NULL,
	[url] [nvarchar](50) NULL,
	[icon] [nvarchar](50) NULL,
 CONSTRAINT [PK_tbl_sssublink] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_sstickets]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_sstickets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[employee_id] [nvarchar](50) NULL,
	[created_on] [datetime] NULL,
	[query] [nvarchar](max) NULL,
	[type] [nvarchar](50) NULL,
	[answer] [nvarchar](max) NULL,
	[ans_date] [datetime] NULL,
	[status] [nvarchar](50) NULL,
 CONSTRAINT [PK_tbl_sstickets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ssticketsreply]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ssticketsreply](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ticket_id] [int] NULL,
	[reply_by] [nvarchar](50) NULL,
	[reply] [nvarchar](max) NULL,
	[answer_date] [datetime] NULL,
 CONSTRAINT [PK_tbl_ssticketsreply] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_answertickets]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_answertickets]
	-- Add the parameters for the stored procedure here
	@Id INT,
	@answer NVARCHAR(MAX),
	@ans_data DATETIME,
	@status NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE tbl_sstickets SET answer = @answer,ans_date = @ans_data,status = @status WHERE Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_getsslogin]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_getsslogin]
	-- Add the parameters for the stored procedure here
	@employeeid NVARCHAR(50),
	@password NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT emp_id,password FROM tbl_sslogin WHERE emp_id = @employeeid AND password = @password
END
GO
/****** Object:  StoredProcedure [dbo].[sp_getssticketreply]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_getssticketreply] 
	-- Add the parameters for the stored procedure here
	@ticketid INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM tbl_ssticketsreply WHERE ticket_id = @ticketid
END
GO
/****** Object:  StoredProcedure [dbo].[sp_getticketsview]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_getticketsview] 
	-- Add the parameters for the stored procedure here
	@empid NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM tbl_sstickets WHERE employee_id = @empid ORDER BY Id DESC
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ssgetadmintickets]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ssgetadmintickets] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM tbl_sstickets ORDER BY ID  DESC 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ssgetlink]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ssgetlink] 
	-- Add the parameters for the stored procedure here
	@role NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT l.Id,l.link,s.Id As SubLinkId,s.sub_link,S.url,S.icon FROM tbl_sslink l LEFT JOIN tbl_sssublink s ON l.Id = S.link_id WHERE l.role = @role
END

GO
/****** Object:  StoredProcedure [dbo].[sp_ssgetnotification]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ssgetnotification] 
	-- Add the parameters for the stored procedure here
	@emp_in NVARCHAR(50)
	

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM tbl_ssnotification WHERE emp_id = @emp_in AND isread = 0
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ssgetnotificationcount]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ssgetnotificationcount] 
	-- Add the parameters for the stored procedure here
	@emp_in NVARCHAR(50)
	

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT(*) FROM tbl_ssnotification WHERE emp_id = @emp_in AND isread = 0
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ssgetsublink]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ssgetsublink] 
	-- Add the parameters for the stored procedure here
	@linkid INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT sub_link,url,icon FROM tbl_sssublink WHERE link_id = @linkid
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ssgetticketbyid]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ssgetticketbyid] 
	-- Add the parameters for the stored procedure here
	@ticket_id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM tbl_sstickets WHERE Id = @ticket_id
END

GO
/****** Object:  StoredProcedure [dbo].[sp_ssgetunanswer]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ssgetunanswer] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM tbl_sstickets WHERE answer = 'UnAnswered'
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ssmarkasread]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ssmarkasread] 
	-- Add the parameters for the stored procedure here
	@Id INT,
	@markasread BIT
	

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE tbl_ssnotification SET isread = @markasread WHERE Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ssraisetickets]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ssraisetickets] 
	-- Add the parameters for the stored procedure here
	@employeeid NVARCHAR(50),
	@createdon DATETIME,
	@query NVARCHAR(MAX),
	@type NVARCHAR(50),
	@answer NVARCHAR(MAX),
	@ansdate DATETIME,
	@status NVARCHAR(50)


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO tbl_sstickets(employee_id,created_on,query,type,answer,ans_date,status)OUTPUT inserted.Id VALUES(@employeeid,@createdon,@query,@type,@answer,@ansdate,@status)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_sssavereply]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_sssavereply]
	-- Add the parameters for the stored procedure here
	@ticket_id INT,
	@reply_by NVARCHAR(50),
	@reply NVARCHAR(MAX),
	@answer_date DATETIME


	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO tbl_ssticketsreply(ticket_id,reply_by,reply,answer_date)VALUES(@ticket_id,@reply_by,@reply,@answer_date)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_sssavnotification]    Script Date: 03-03-2026 8.02.52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_sssavnotification] 
	-- Add the parameters for the stored procedure here
	@emp_in NVARCHAR(50),
	@message NVARCHAR(500),
	@isread BIT,
	@notifyon DATETIME

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO tbl_ssnotification(emp_id,message,isread,notify_on)VALUES(@emp_in,@message,@isread,@notifyon)
END
GO
