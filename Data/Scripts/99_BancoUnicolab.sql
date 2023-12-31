USE [master]
GO
/****** Object:  Database [UNICOLAB]    Script Date: 31/10/2022 23:49:50 ******/
CREATE DATABASE [UNICOLAB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'UNICOLAB', FILENAME = N'D:\Program Files\MSSQL11.MSSQLSERVER\MSSQL\DATA\UNICOLAB.mdf' , SIZE = 5184KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'UNICOLAB_log', FILENAME = N'D:\Program Files\MSSQL11.MSSQLSERVER\MSSQL\DATA\UNICOLAB_log.ldf' , SIZE = 1040KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [UNICOLAB] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [UNICOLAB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [UNICOLAB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [UNICOLAB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [UNICOLAB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [UNICOLAB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [UNICOLAB] SET ARITHABORT OFF 
GO
ALTER DATABASE [UNICOLAB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [UNICOLAB] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [UNICOLAB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [UNICOLAB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [UNICOLAB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [UNICOLAB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [UNICOLAB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [UNICOLAB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [UNICOLAB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [UNICOLAB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [UNICOLAB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [UNICOLAB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [UNICOLAB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [UNICOLAB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [UNICOLAB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [UNICOLAB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [UNICOLAB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [UNICOLAB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [UNICOLAB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [UNICOLAB] SET  MULTI_USER 
GO
ALTER DATABASE [UNICOLAB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [UNICOLAB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [UNICOLAB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [UNICOLAB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [UNICOLAB]
GO
/****** Object:  User [unicolab_user]    Script Date: 31/10/2022 23:49:51 ******/
CREATE USER [unicolab_user] FOR LOGIN [unicolab_user] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [unicolab_user]
GO
/****** Object:  FullTextCatalog [Catalogo]    Script Date: 31/10/2022 23:49:51 ******/
CREATE FULLTEXT CATALOG [Catalogo]WITH ACCENT_SENSITIVITY = ON
AS DEFAULT

GO
/****** Object:  Table [dbo].[AREA]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AREA](
	[ID] [nvarchar](40) NOT NULL,
	[DESCRICAO] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_ID_AREA] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CLASSIFICACAORESPOSTA]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLASSIFICACAORESPOSTA](
	[ID] [nvarchar](40) NOT NULL,
	[CLASSIFICACAO] [int] NOT NULL,
	[USUARIOID] [nvarchar](40) NOT NULL,
	[RESPOSTAID] [nvarchar](40) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CURSO]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CURSO](
	[ID] [nvarchar](40) NOT NULL,
	[AREAID] [nvarchar](40) NOT NULL,
	[DESCRICAO] [nvarchar](255) NOT NULL,
	[PONTOS] [int] NULL,
	[ATIVO] [bit] NULL,
 CONSTRAINT [PK_ID_CURSO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DUVIDA]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DUVIDA](
	[ID] [nvarchar](40) NOT NULL,
	[PERGUNTA] [nvarchar](4000) NOT NULL,
	[PONTOS] [int] NOT NULL,
	[DATAHORA] [datetime] NOT NULL,
	[USUARIOID] [nvarchar](40) NOT NULL,
	[MATERIAID] [nvarchar](40) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EVENTO]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EVENTO](
	[ID] [nvarchar](40) NOT NULL,
	[CURSOID] [nvarchar](40) NULL,
	[USUARIOID] [nvarchar](40) NULL,
	[TITULO] [nvarchar](255) NOT NULL,
	[TIPOEVENTOID] [nvarchar](40) NOT NULL,
	[DESCRICAO] [nvarchar](255) NOT NULL,
	[PONTOS] [int] NULL,
	[PALESTRANTE] [nvarchar](255) NOT NULL,
	[LOCALEVENTO] [nvarchar](255) NOT NULL,
	[DATAHORARIOINICIO] [datetime] NOT NULL,
	[DURACAO] [int] NOT NULL,
	[ATIVO] [bit] NULL,
 CONSTRAINT [Pk_Id_Eventos] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EVENTOCURSO]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EVENTOCURSO](
	[ID] [nvarchar](40) NOT NULL,
	[CURSOID] [nvarchar](40) NOT NULL,
	[EVENTOID] [nvarchar](40) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LINKIMPORTANTE]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LINKIMPORTANTE](
	[ID] [nvarchar](40) NOT NULL,
	[TITULO] [nvarchar](255) NOT NULL,
	[DESCRICAO] [nvarchar](255) NOT NULL,
	[URL] [nvarchar](255) NOT NULL,
	[ICONE] [nvarchar](255) NOT NULL,
	[ORDEM] [int] NOT NULL,
	[ATIVO] [bit] NULL,
 CONSTRAINT [PK_ID_LINKIMPORTANTE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Log]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Aplicacao] [nvarchar](255) NOT NULL,
	[DataHora] [datetime] NOT NULL,
	[Nivel] [nvarchar](255) NOT NULL,
	[Mensagem] [nvarchar](max) NOT NULL,
	[Origem] [nvarchar](255) NULL,
	[Endereco] [nvarchar](max) NULL,
	[Excecao] [nvarchar](max) NULL,
	[Usuario] [nvarchar](255) NULL,
 CONSTRAINT [PK_dbo.Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MATERIA]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MATERIA](
	[ID] [nvarchar](40) NOT NULL,
	[DESCRICAO] [nvarchar](255) NOT NULL,
	[SEMESTRESUGERIDO] [int] NOT NULL,
 CONSTRAINT [Pk_ID_Materia] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MATERIACURSO]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MATERIACURSO](
	[ID] [nvarchar](40) NOT NULL,
	[MATERIAID] [nvarchar](40) NOT NULL,
	[CURSOID] [nvarchar](40) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MODULO]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MODULO](
	[ID] [nvarchar](40) NOT NULL,
	[TITULO] [nvarchar](255) NOT NULL,
	[DESCRICAO] [nvarchar](255) NOT NULL,
	[URL] [nvarchar](255) NOT NULL,
	[ICONE] [nvarchar](255) NOT NULL,
	[ORDEM] [int] NOT NULL,
	[ATIVO] [bit] NULL,
 CONSTRAINT [PK_ID_MODULO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OPORTUNIDADE]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OPORTUNIDADE](
	[ID] [nvarchar](40) NOT NULL,
	[CURSOID] [nvarchar](40) NOT NULL,
	[USUARIOID] [nvarchar](40) NOT NULL,
	[TITULO] [nvarchar](255) NOT NULL,
	[EMPRESA] [nvarchar](255) NOT NULL,
	[DESCRICAO] [nvarchar](4000) NOT NULL,
	[SALARIO] [decimal](18, 2) NULL,
	[DATAINICIO] [date] NOT NULL,
	[DATAFIM] [date] NULL,
	[ATIVO] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PERFIL]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PERFIL](
	[ID] [nvarchar](40) NOT NULL,
	[DESCRICAO] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_ID_PERFIL] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PERFILMODULO]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PERFILMODULO](
	[ID] [nvarchar](40) NOT NULL,
	[PERFILID] [nvarchar](40) NOT NULL,
	[MODULOID] [nvarchar](40) NOT NULL,
	[ACESSO] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_ID_PERFILMODULO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RESPOSTA]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RESPOSTA](
	[ID] [nvarchar](40) NOT NULL,
	[DESCRICAO] [varchar](8000) NOT NULL,
	[MELHORRESPOSTA] [bit] NOT NULL,
	[DATAHORA] [datetime] NOT NULL,
	[USUARIOID] [nvarchar](40) NOT NULL,
	[DUVIDAID] [nvarchar](40) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TIPOEVENTO]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TIPOEVENTO](
	[ID] [nvarchar](40) NOT NULL,
	[NOMETIPO] [nvarchar](100) NOT NULL,
	[DESCRICAO] [nvarchar](255) NOT NULL,
 CONSTRAINT [Pk_ID_TipoEvento] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[USUARIO]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USUARIO](
	[ID] [nvarchar](40) NOT NULL,
	[NOME] [nvarchar](100) NOT NULL,
	[SENHA] [nvarchar](255) NULL,
	[EMAIL] [nvarchar](255) NULL,
	[LOGIN] [nvarchar](255) NOT NULL,
	[ATIVO] [bit] NOT NULL,
	[PRIMEIROACESSO] [bit] NULL,
	[ULTIMOACESSO] [datetime] NOT NULL,
	[TOKEN] [nvarchar](255) NULL,
	[DATAULTIMOTOKEN] [datetime] NOT NULL,
	[PERFILID] [nvarchar](40) NOT NULL,
	[RESETSOLICITADO] [bit] NOT NULL,
	[PONTOS] [int] NULL,
	[RA] [nvarchar](10) NULL,
 CONSTRAINT [PK_ID_USUARIO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[USUARIOCURSO]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USUARIOCURSO](
	[ID] [nvarchar](40) NOT NULL,
	[USUARIOID] [nvarchar](40) NOT NULL,
	[CURSOID] [nvarchar](40) NOT NULL,
	[ATIVO] [bit] NULL,
 CONSTRAINT [PK_ID_USUARIOCURSO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[USUARIOMATERIA]    Script Date: 31/10/2022 23:49:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USUARIOMATERIA](
	[ID] [nvarchar](40) NOT NULL,
	[USUARIOID] [nvarchar](40) NOT NULL,
	[MATERIAID] [nvarchar](40) NOT NULL,
	[SEMESTRE] [int] NOT NULL,
	[ANO] [int] NOT NULL,
	[CONCLUIDO] [bit] NULL,
	[MEDIA] [int] NULL,
 CONSTRAINT [Pk_ID_UsuarioMateria] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Ind_Per]    Script Date: 31/10/2022 23:49:51 ******/
CREATE NONCLUSTERED INDEX [Ind_Per] ON [dbo].[DUVIDA]
(
	[PERGUNTA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Index_Pergunta]    Script Date: 31/10/2022 23:49:51 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Index_Pergunta] ON [dbo].[DUVIDA]
(
	[PERGUNTA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CURSO] ADD  DEFAULT ((1)) FOR [ATIVO]
GO
ALTER TABLE [dbo].[EVENTO] ADD  DEFAULT ((1)) FOR [ATIVO]
GO
ALTER TABLE [dbo].[LINKIMPORTANTE] ADD  DEFAULT ((1)) FOR [ATIVO]
GO
ALTER TABLE [dbo].[MODULO] ADD  DEFAULT ((1)) FOR [ATIVO]
GO
ALTER TABLE [dbo].[OPORTUNIDADE] ADD  DEFAULT ((1)) FOR [ATIVO]
GO
ALTER TABLE [dbo].[RESPOSTA] ADD  DEFAULT ((0)) FOR [MELHORRESPOSTA]
GO
ALTER TABLE [dbo].[USUARIO] ADD  DEFAULT ('') FOR [SENHA]
GO
ALTER TABLE [dbo].[USUARIO] ADD  DEFAULT ((1)) FOR [PRIMEIROACESSO]
GO
ALTER TABLE [dbo].[USUARIO] ADD  DEFAULT ('') FOR [TOKEN]
GO
ALTER TABLE [dbo].[USUARIOCURSO] ADD  DEFAULT ((1)) FOR [ATIVO]
GO
ALTER TABLE [dbo].[CLASSIFICACAORESPOSTA]  WITH CHECK ADD FOREIGN KEY([RESPOSTAID])
REFERENCES [dbo].[RESPOSTA] ([ID])
GO
ALTER TABLE [dbo].[CLASSIFICACAORESPOSTA]  WITH CHECK ADD FOREIGN KEY([USUARIOID])
REFERENCES [dbo].[USUARIO] ([ID])
GO
ALTER TABLE [dbo].[CURSO]  WITH CHECK ADD  CONSTRAINT [FK_AreaId_Curso] FOREIGN KEY([AREAID])
REFERENCES [dbo].[AREA] ([ID])
GO
ALTER TABLE [dbo].[CURSO] CHECK CONSTRAINT [FK_AreaId_Curso]
GO
ALTER TABLE [dbo].[DUVIDA]  WITH CHECK ADD FOREIGN KEY([MATERIAID])
REFERENCES [dbo].[MATERIA] ([ID])
GO
ALTER TABLE [dbo].[DUVIDA]  WITH CHECK ADD FOREIGN KEY([USUARIOID])
REFERENCES [dbo].[USUARIO] ([ID])
GO
ALTER TABLE [dbo].[EVENTO]  WITH CHECK ADD  CONSTRAINT [Fk_CursoId_Eventos] FOREIGN KEY([CURSOID])
REFERENCES [dbo].[CURSO] ([ID])
GO
ALTER TABLE [dbo].[EVENTO] CHECK CONSTRAINT [Fk_CursoId_Eventos]
GO
ALTER TABLE [dbo].[EVENTO]  WITH CHECK ADD  CONSTRAINT [Fk_TipoEventoId_Eventos] FOREIGN KEY([TIPOEVENTOID])
REFERENCES [dbo].[TIPOEVENTO] ([ID])
GO
ALTER TABLE [dbo].[EVENTO] CHECK CONSTRAINT [Fk_TipoEventoId_Eventos]
GO
ALTER TABLE [dbo].[EVENTO]  WITH CHECK ADD  CONSTRAINT [Fk_UsuarioId_Eventos] FOREIGN KEY([USUARIOID])
REFERENCES [dbo].[USUARIO] ([ID])
GO
ALTER TABLE [dbo].[EVENTO] CHECK CONSTRAINT [Fk_UsuarioId_Eventos]
GO
ALTER TABLE [dbo].[EVENTOCURSO]  WITH CHECK ADD FOREIGN KEY([CURSOID])
REFERENCES [dbo].[CURSO] ([ID])
GO
ALTER TABLE [dbo].[EVENTOCURSO]  WITH CHECK ADD FOREIGN KEY([EVENTOID])
REFERENCES [dbo].[EVENTO] ([ID])
GO
ALTER TABLE [dbo].[MATERIACURSO]  WITH CHECK ADD FOREIGN KEY([CURSOID])
REFERENCES [dbo].[CURSO] ([ID])
GO
ALTER TABLE [dbo].[MATERIACURSO]  WITH CHECK ADD FOREIGN KEY([MATERIAID])
REFERENCES [dbo].[MATERIA] ([ID])
GO
ALTER TABLE [dbo].[OPORTUNIDADE]  WITH CHECK ADD FOREIGN KEY([CURSOID])
REFERENCES [dbo].[CURSO] ([ID])
GO
ALTER TABLE [dbo].[OPORTUNIDADE]  WITH CHECK ADD FOREIGN KEY([USUARIOID])
REFERENCES [dbo].[USUARIO] ([ID])
GO
ALTER TABLE [dbo].[PERFILMODULO]  WITH CHECK ADD  CONSTRAINT [FK_ModuloId_PerfilModulo] FOREIGN KEY([MODULOID])
REFERENCES [dbo].[MODULO] ([ID])
GO
ALTER TABLE [dbo].[PERFILMODULO] CHECK CONSTRAINT [FK_ModuloId_PerfilModulo]
GO
ALTER TABLE [dbo].[PERFILMODULO]  WITH CHECK ADD  CONSTRAINT [FK_PerfilId_PerfilModulo] FOREIGN KEY([PERFILID])
REFERENCES [dbo].[PERFIL] ([ID])
GO
ALTER TABLE [dbo].[PERFILMODULO] CHECK CONSTRAINT [FK_PerfilId_PerfilModulo]
GO
ALTER TABLE [dbo].[RESPOSTA]  WITH CHECK ADD FOREIGN KEY([DUVIDAID])
REFERENCES [dbo].[DUVIDA] ([ID])
GO
ALTER TABLE [dbo].[RESPOSTA]  WITH CHECK ADD FOREIGN KEY([USUARIOID])
REFERENCES [dbo].[USUARIO] ([ID])
GO
ALTER TABLE [dbo].[USUARIO]  WITH CHECK ADD  CONSTRAINT [FK_PerfilId_Usuario] FOREIGN KEY([PERFILID])
REFERENCES [dbo].[PERFIL] ([ID])
GO
ALTER TABLE [dbo].[USUARIO] CHECK CONSTRAINT [FK_PerfilId_Usuario]
GO
ALTER TABLE [dbo].[USUARIOCURSO]  WITH CHECK ADD  CONSTRAINT [FK_CURSOID_CURSO] FOREIGN KEY([CURSOID])
REFERENCES [dbo].[CURSO] ([ID])
GO
ALTER TABLE [dbo].[USUARIOCURSO] CHECK CONSTRAINT [FK_CURSOID_CURSO]
GO
ALTER TABLE [dbo].[USUARIOCURSO]  WITH CHECK ADD  CONSTRAINT [FK_USUARIOID_USUARIO] FOREIGN KEY([USUARIOID])
REFERENCES [dbo].[USUARIO] ([ID])
GO
ALTER TABLE [dbo].[USUARIOCURSO] CHECK CONSTRAINT [FK_USUARIOID_USUARIO]
GO
ALTER TABLE [dbo].[USUARIOMATERIA]  WITH CHECK ADD  CONSTRAINT [Fk_MateriaId_UsuarioMateria] FOREIGN KEY([MATERIAID])
REFERENCES [dbo].[MATERIA] ([ID])
GO
ALTER TABLE [dbo].[USUARIOMATERIA] CHECK CONSTRAINT [Fk_MateriaId_UsuarioMateria]
GO
ALTER TABLE [dbo].[USUARIOMATERIA]  WITH CHECK ADD  CONSTRAINT [Fk_UsuarioId_UsuarioMateria] FOREIGN KEY([USUARIOID])
REFERENCES [dbo].[USUARIO] ([ID])
GO
ALTER TABLE [dbo].[USUARIOMATERIA] CHECK CONSTRAINT [Fk_UsuarioId_UsuarioMateria]
GO
USE [master]
GO
ALTER DATABASE [UNICOLAB] SET  READ_WRITE 
GO
