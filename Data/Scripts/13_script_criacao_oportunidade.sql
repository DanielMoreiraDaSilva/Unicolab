﻿CREATE TABLE dbo . OPORTUNIDADE(
	 ID nvarchar(40) NOT NULL PRIMARY KEY,
	 CURSOID nvarchar(40) NOT NULL FOREIGN KEY REFERENCES CURSO (ID), /* CHAVE ESTRANGEIRA DA TABELA CURSO, RESPONSÁVEL PELOS CURSOS MINISTRADOS NA INSTITUIÇÃO */
	 USUARIOID nvarchar(40) NOT NULL FOREIGN KEY REFERENCES USUARIO (ID), /* CHAVE ESTRANGEIRA DA TABELA USUARIO, USUARIO QUE CADASTROU A OPORTUNIDADE */
	 TITULO nvarchar(255) NOT NULL,
	 EMPRESA nvarchar(255) NOT NULL,
	 DESCRICAO nvarchar(255) NOT NULL,
	 SALARIO decimal(18,2), /* VARCHAR POR QUE O SALÁRIO PODE SER A COMBINAR */
	 DATAINICIO date NOT NULL, 
	 DATAFIM date, /* OPÇÃO PARA VAGAS COM PROCESSO SELETIVO */
	 ATIVO bit NULL DEFAULT((1))
) 
GO