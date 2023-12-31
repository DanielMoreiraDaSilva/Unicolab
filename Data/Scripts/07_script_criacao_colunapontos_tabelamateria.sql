﻿ALTER TABLE USUARIO
ADD	Pontos INT NULL;

CREATE TABLE USUARIOMATERIA(
	ID NVARCHAR(40) NOT NULL,
	CONSTRAINT Pk_ID_UsuarioMateria PRIMARY KEY (ID),
	USUARIOID NVARCHAR(40) NOT NULL,
	MATERIAID NVARCHAR(40) NOT NULL,
	SEMESTRE INT NOT NULL,
	ANO INT NOT NULL,
	CONCLUIDO BIT NULL,
	MEDIA INT NULL
)

GO

CREATE TABLE MATERIA(
	ID NVARCHAR(40) NOT NULL,
	CONSTRAINT Pk_ID_Materia PRIMARY KEY (ID),
	DESCRICAO NVARCHAR(255) NOT NULL,
	SEMESTRESUGERIDO INT NOT NULL
)

GO

ALTER TABLE USUARIOMATERIA
ADD CONSTRAINT Fk_UsuarioId_UsuarioMateria FOREIGN KEY (USUARIOID) REFERENCES USUARIO (ID);

ALTER TABLE USUARIOMATERIA
ADD CONSTRAINT Fk_MateriaId_UsuarioMateria FOREIGN KEY (MATERIAID) REFERENCES MATERIA (ID);