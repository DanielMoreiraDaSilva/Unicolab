USE master
CREATE DATABASE UNICOLAB
GO
CREATE LOGIN unicolab_user WITH PASSWORD = '123';
GO
USE UNICOLAB
GO
create user unicolab_user for login unicolab_user
GO
EXEC sp_addrolemember N'db_owner', N'unicolab_user'
GO

CREATE TABLE [Log](
	Id int IDENTITY(1,1) NOT NULL,
	Aplicacao nvarchar(255) NOT NULL,
	DataHora datetime NOT NULL,
	Nivel nvarchar(255) NOT NULL,
	Mensagem nvarchar(max) NOT NULL,
	Origem nvarchar(255) NULL,
	Endereco nvarchar(max) NULL,
	Excecao nvarchar(max) NULL,
	Usuario nvarchar(255) NULL
	CONSTRAINT [PK_dbo.Log] PRIMARY KEY CLUSTERED ( Id ASC) 
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO