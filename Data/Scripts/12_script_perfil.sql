﻿ALTER TABLE USUARIO
ADD RA NVARCHAR(10) NULL;

CREATE TABLE AREA(
	ID NVARCHAR(40) NOT NULL,
	DESCRICAO NVARCHAR(255) NOT NULL
	)
GO

ALTER TABLE AREA
ADD CONSTRAINT PK_ID_AREA PRIMARY KEY (ID);

CREATE TABLE CURSO(
	ID NVARCHAR(40) NOT NULL,
	AREAID NVARCHAR(40) NOT NULL,
	DESCRICAO NVARCHAR(255) NOT NULL, 
	PONTOS INT NULL,
	ATIVO BIT DEFAULT 1
	)
GO

ALTER TABLE CURSO
ADD CONSTRAINT PK_ID_CURSO PRIMARY KEY (ID);

ALTER TABLE	CURSO
ADD CONSTRAINT FK_AreaId_Curso FOREIGN KEY (AREAID) REFERENCES AREA (ID);

CREATE TABLE USUARIOCURSO(
	ID NVARCHAR(40) NOT NULL,
	USUARIOID NVARCHAR(40) NOT NULL,
	CURSOID NVARCHAR(40) NOT NULL,
	ATIVO BIT DEFAULT 1
	)
GO

ALTER TABLE USUARIOCURSO
ADD CONSTRAINT PK_ID_USUARIOCURSO PRIMARY KEY (ID);

ALTER TABLE	USUARIOCURSO
ADD CONSTRAINT FK_USUARIOID_USUARIO FOREIGN KEY (USUARIOID) REFERENCES USUARIO (ID);

ALTER TABLE	USUARIOCURSO
ADD CONSTRAINT FK_CURSOID_CURSO FOREIGN KEY (CURSOID) REFERENCES CURSO (ID);
