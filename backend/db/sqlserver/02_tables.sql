USE [CampusNovaDB]
GO

/* 02. Esquemas, tablas, llaves, valores por defecto y restricciones. Ejecutar una sola vez sobre base vacia. */

IF SCHEMA_ID('academic') IS NULL EXEC('CREATE SCHEMA [academic]');
GO
IF SCHEMA_ID('security') IS NULL EXEC('CREATE SCHEMA [security]');
GO
/****** Objeto: Table [academic].[institution] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [academic].[institution](
	[id_pk] [int] IDENTITY(1,1) NOT NULL,
	[code] [bigint] NOT NULL,
	[name] [nvarchar](150) NOT NULL,
	[address] [nvarchar](200) NULL,
	[city] [nvarchar](100) NULL,
	[email] [nvarchar](256) NULL,
	[phone] [varchar](30) NULL,
	[active] [bit] NOT NULL,
	[created_at] [datetime2](3) NOT NULL,
 CONSTRAINT [PK_institution] PRIMARY KEY CLUSTERED 
(
	[id_pk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UX_institution_code] UNIQUE NONCLUSTERED 
(
	[code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [academic].[student] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [academic].[student](
	[id_pk] [int] IDENTITY(1,1) NOT NULL,
	[id_fk_institution] [int] NOT NULL,
	[identification] [bigint] NOT NULL,
	[first_name] [nvarchar](100) NOT NULL,
	[last_name] [nvarchar](100) NOT NULL,
	[email] [nvarchar](256) NULL,
	[phone] [varchar](30) NULL,
	[active] [bit] NOT NULL,
	[created_at] [datetime2](3) NOT NULL,
 CONSTRAINT [PK_student] PRIMARY KEY CLUSTERED 
(
	[id_pk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UX_student_identification] UNIQUE NONCLUSTERED 
(
	[id_fk_institution] ASC,
	[identification] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UX_student_institution] UNIQUE NONCLUSTERED 
(
	[id_pk] ASC,
	[id_fk_institution] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [security].[users] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [security].[users](
	[id_pk] [int] IDENTITY(1,1) NOT NULL,
	[id_fk_institution] [int] NOT NULL,
	[username] [bigint] NOT NULL,
	[password_hash] [char](64) NOT NULL,
	[active] [bit] NOT NULL,
	[created_at] [datetime2](3) NOT NULL,
	[id_fk_role] [tinyint] NOT NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[id_pk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UX_users_username] UNIQUE NONCLUSTERED 
(
	[id_fk_institution] ASC,
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [security].[role] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [security].[role](
	[id_pk] [tinyint] NOT NULL,
	[name] [varchar](20) NOT NULL,
 CONSTRAINT [PK_role] PRIMARY KEY CLUSTERED 
(
	[id_pk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UX_role_name] UNIQUE NONCLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [academic].[professor] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [academic].[professor](
	[id_pk] [int] IDENTITY(1,1) NOT NULL,
	[id_fk_institution] [int] NOT NULL,
	[identification] [bigint] NOT NULL,
	[first_name] [nvarchar](100) NOT NULL,
	[last_name] [nvarchar](100) NOT NULL,
	[email] [nvarchar](256) NULL,
	[phone] [varchar](30) NULL,
	[active] [bit] NOT NULL,
	[created_at] [datetime2](3) NOT NULL,
 CONSTRAINT [PK_professor] PRIMARY KEY CLUSTERED 
(
	[id_pk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UX_professor_identification] UNIQUE NONCLUSTERED 
(
	[id_fk_institution] ASC,
	[identification] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UX_professor_institution] UNIQUE NONCLUSTERED 
(
	[id_pk] ASC,
	[id_fk_institution] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [academic].[student_subject] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [academic].[student_subject](
	[id_fk_institution] [int] NOT NULL,
	[id_fk_student] [int] NOT NULL,
	[id_fk_subject] [int] NOT NULL,
	[enrolled_at] [datetime2](3) NOT NULL,
	[id_pk] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_student_subject] PRIMARY KEY CLUSTERED 
(
	[id_pk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UX_student_subject] UNIQUE NONCLUSTERED 
(
	[id_fk_institution] ASC,
	[id_fk_student] ASC,
	[id_fk_subject] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [academic].[subject] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [academic].[subject](
	[id_pk] [int] IDENTITY(1,1) NOT NULL,
	[id_fk_institution] [int] NOT NULL,
	[id_fk_professor] [int] NOT NULL,
	[code] [varchar](10) NOT NULL,
	[name] [nvarchar](120) NOT NULL,
	[description] [nvarchar](255) NULL,
	[credits] [tinyint] NOT NULL,
	[active] [bit] NOT NULL,
	[created_at] [datetime2](3) NOT NULL,
 CONSTRAINT [PK_subject] PRIMARY KEY CLUSTERED 
(
	[id_pk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UX_subject_code] UNIQUE NONCLUSTERED 
(
	[id_fk_institution] ASC,
	[code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UX_subject_institution] UNIQUE NONCLUSTERED 
(
	[id_pk] ASC,
	[id_fk_institution] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [security].[audit_log] Fecha de script: 25/10/2026 1:32:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [security].[audit_log](
	[id_pk] [int] IDENTITY(1,1) NOT NULL,
	[data_json] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_audit_log] PRIMARY KEY CLUSTERED 
(
	[id_pk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [academic].[institution] ADD  CONSTRAINT [DF_institution_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [academic].[institution] ADD  CONSTRAINT [DF_institution_created]  DEFAULT (sysutcdatetime()) FOR [created_at]
GO
ALTER TABLE [academic].[professor] ADD  CONSTRAINT [DF_professor_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [academic].[professor] ADD  CONSTRAINT [DF_professor_created]  DEFAULT (sysutcdatetime()) FOR [created_at]
GO
ALTER TABLE [academic].[student] ADD  CONSTRAINT [DF_student_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [academic].[student] ADD  CONSTRAINT [DF_student_created]  DEFAULT (sysutcdatetime()) FOR [created_at]
GO
ALTER TABLE [academic].[student_subject] ADD  CONSTRAINT [DF_student_subject_enrolled]  DEFAULT (sysutcdatetime()) FOR [enrolled_at]
GO
ALTER TABLE [academic].[subject] ADD  CONSTRAINT [DF_subject_credits]  DEFAULT ((3)) FOR [credits]
GO
ALTER TABLE [academic].[subject] ADD  CONSTRAINT [DF_subject_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [academic].[subject] ADD  CONSTRAINT [DF_subject_created]  DEFAULT (sysutcdatetime()) FOR [created_at]
GO
ALTER TABLE [security].[users] ADD  CONSTRAINT [DF_users_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [security].[users] ADD  CONSTRAINT [DF_users_created]  DEFAULT (sysutcdatetime()) FOR [created_at]
GO
ALTER TABLE [security].[users] ADD  CONSTRAINT [DF_users_role]  DEFAULT ((1)) FOR [id_fk_role]
GO
ALTER TABLE [academic].[professor]  WITH CHECK ADD  CONSTRAINT [FK_professor_institution] FOREIGN KEY([id_fk_institution])
REFERENCES [academic].[institution] ([id_pk])
GO
ALTER TABLE [academic].[professor] CHECK CONSTRAINT [FK_professor_institution]
GO
ALTER TABLE [academic].[student]  WITH CHECK ADD  CONSTRAINT [FK_student_institution] FOREIGN KEY([id_fk_institution])
REFERENCES [academic].[institution] ([id_pk])
GO
ALTER TABLE [academic].[student] CHECK CONSTRAINT [FK_student_institution]
GO
ALTER TABLE [academic].[student_subject]  WITH CHECK ADD  CONSTRAINT [FK_student_subject_institution] FOREIGN KEY([id_fk_institution])
REFERENCES [academic].[institution] ([id_pk])
GO
ALTER TABLE [academic].[student_subject] CHECK CONSTRAINT [FK_student_subject_institution]
GO
ALTER TABLE [academic].[student_subject]  WITH CHECK ADD  CONSTRAINT [FK_student_subject_student] FOREIGN KEY([id_fk_student], [id_fk_institution])
REFERENCES [academic].[student] ([id_pk], [id_fk_institution])
GO
ALTER TABLE [academic].[student_subject] CHECK CONSTRAINT [FK_student_subject_student]
GO
ALTER TABLE [academic].[student_subject]  WITH CHECK ADD  CONSTRAINT [FK_student_subject_subject] FOREIGN KEY([id_fk_subject], [id_fk_institution])
REFERENCES [academic].[subject] ([id_pk], [id_fk_institution])
GO
ALTER TABLE [academic].[student_subject] CHECK CONSTRAINT [FK_student_subject_subject]
GO
ALTER TABLE [academic].[subject]  WITH CHECK ADD  CONSTRAINT [FK_subject_institution] FOREIGN KEY([id_fk_institution])
REFERENCES [academic].[institution] ([id_pk])
GO
ALTER TABLE [academic].[subject] CHECK CONSTRAINT [FK_subject_institution]
GO
ALTER TABLE [academic].[subject]  WITH CHECK ADD  CONSTRAINT [FK_subject_professor] FOREIGN KEY([id_fk_professor], [id_fk_institution])
REFERENCES [academic].[professor] ([id_pk], [id_fk_institution])
GO
ALTER TABLE [academic].[subject] CHECK CONSTRAINT [FK_subject_professor]
GO
ALTER TABLE [security].[users]  WITH CHECK ADD  CONSTRAINT [FK_users_institution] FOREIGN KEY([id_fk_institution])
REFERENCES [academic].[institution] ([id_pk])
GO
ALTER TABLE [security].[users] CHECK CONSTRAINT [FK_users_institution]
GO
ALTER TABLE [security].[users]  WITH CHECK ADD  CONSTRAINT [FK_users_role] FOREIGN KEY([id_fk_role])
REFERENCES [security].[role] ([id_pk])
GO
ALTER TABLE [security].[users] CHECK CONSTRAINT [FK_users_role]
GO
ALTER TABLE [security].[users]  WITH CHECK ADD  CONSTRAINT [FK_users_student] FOREIGN KEY([id_fk_institution], [username])
REFERENCES [academic].[student] ([id_fk_institution], [identification])
GO
ALTER TABLE [security].[users] CHECK CONSTRAINT [FK_users_student]
GO
ALTER TABLE [security].[audit_log]  WITH CHECK ADD  CONSTRAINT [CK_audit_log_json] CHECK  ((isjson([data_json])=(1)))
GO
ALTER TABLE [security].[audit_log] CHECK CONSTRAINT [CK_audit_log_json]
GO
