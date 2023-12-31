﻿ALTER TABLE USUARIO
ADD CONSTRAINT PK_ID_USUARIO PRIMARY KEY (ID);

ALTER TABLE PERFILMODULO
ADD CONSTRAINT PK_ID_PERFILMODULO PRIMARY KEY (ID);

ALTER TABLE PERFIL
ADD CONSTRAINT PK_ID_PERFIL PRIMARY KEY (ID);

ALTER TABLE MODULO
ADD CONSTRAINT PK_ID_MODULO PRIMARY KEY (ID);

ALTER TABLE PERFILMODULO 
ALTER COLUMN PERFILID NVARCHAR (40) NOT NULL;

ALTER TABLE PERFILMODULO 
ALTER COLUMN MODULOID NVARCHAR (40) NOT NULL;

ALTER TABLE	USUARIO
ADD CONSTRAINT FK_PerfilId_Usuario FOREIGN KEY (PERFILID) REFERENCES PERFIL (ID);

ALTER TABLE	PERFILMODULO
ADD CONSTRAINT FK_PerfilId_PerfilModulo FOREIGN KEY (PERFILID) REFERENCES PERFIL (ID);

ALTER TABLE	PERFILMODULO
ADD CONSTRAINT FK_ModuloId_PerfilModulo FOREIGN KEY (MODULOID) REFERENCES MODULO (ID);