exec sp_RENAME 'USUARIO.Pontos','PONTOS','COLUMN'

UPDATE USUARIO SET PERFILID='6D672B19-8F61-413E-8277-3351DDDC5C56'

ALTER TABLE LINKIMPORTANTE
ADD CONSTRAINT PK_ID_LINKIMPORTANTE PRIMARY KEY (ID);

INSERT INTO [dbo].[MODULO]
     VALUES
           ('A7717180-B9EC-49B2-90C4-94614B3EC957'
           ,'Dados Mestre'
           ,'Gestão do sistema'
           ,'/dados-mestre'
           ,'mdi-cogs'
           ,6
           ,1)
GO

INSERT INTO [dbo].[PERFILMODULO]
           ([ID]
           ,[PERFILID]
           ,[MODULOID]
           ,[ACESSO])
     VALUES
           ('C8DB5A5A-07DA-4028-95E2-577FE68E89B9'
           ,'6D672B19-8F61-413E-8277-3351DDDC5C56'
           ,'A7717180-B9EC-49B2-90C4-94614B3EC957'
           ,'All')
GO