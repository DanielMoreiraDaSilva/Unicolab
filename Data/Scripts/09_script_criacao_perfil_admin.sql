INSERT INTO [dbo].[PERFIL]
VALUES
('6D672B19-8F61-413E-8277-3351DDDC5C56'
,'Admin')
GO

INSERT INTO [dbo].[MODULO]
     VALUES
           ('C4CDD573-CE20-41BF-9B08-A76CB4A7F99E'
           ,'Dúvidas'
           ,'Centro de dúvidas e respostas'
           ,'/duvidas'
           ,'mdi-comment-processing-outline'
           ,1
           ,1)
GO

INSERT INTO [dbo].[MODULO]
     VALUES
           ('B3AE0F40-9ED1-414A-A47B-68C9250BF621'
           ,'Contatos'
           ,'Lista de contatos'
           ,'/contatos'
           ,'mdi-account-multiple-outline'
           ,2
           ,1)
GO

INSERT INTO [dbo].[MODULO]
     VALUES
           ('6B8BCCBB-A9ED-41F3-9B05-A6245AE51B6A'
           ,'Links Importantes'
           ,'Lista de links importantes'
           ,'/links-importantes'
           ,'mdi-flash-outline'
           ,3
           ,1)
GO

INSERT INTO [dbo].[MODULO]
     VALUES
           ('74EBB025-46DC-4D29-B9B9-42B4FE34557B'
           ,'Oportunidades'
           ,'Aba de oportunidades'
           ,'/oportunidades'
           ,'mdi-briefcase-outline'
           ,4
           ,1)
GO

INSERT INTO [dbo].[MODULO]
     VALUES
           ('4F01042A-4DC0-4794-85F1-E9014341F1BA'
           ,'Eventos'
           ,'Aba de eventos'
           ,'/eventos'
           ,'mdi-calendar-blank-outline'
           ,5
           ,1)
GO

INSERT INTO [dbo].[PERFILMODULO]
           ([ID]
           ,[PERFILID]
           ,[MODULOID]
           ,[ACESSO])
     VALUES
           ('5632A6BD-284A-4518-91CB-03A3F6EBE8C5'
           ,'6D672B19-8F61-413E-8277-3351DDDC5C56'
           ,'C4CDD573-CE20-41BF-9B08-A76CB4A7F99E'
           ,'All')
GO


INSERT INTO [dbo].[PERFILMODULO]
           ([ID]
           ,[PERFILID]
           ,[MODULOID]
           ,[ACESSO])
     VALUES
           ('06ED6202-0D01-4A26-8508-1866345405FB'
           ,'6D672B19-8F61-413E-8277-3351DDDC5C56'
           ,'B3AE0F40-9ED1-414A-A47B-68C9250BF621'
           ,'All')
GO

INSERT INTO [dbo].[PERFILMODULO]
           ([ID]
           ,[PERFILID]
           ,[MODULOID]
           ,[ACESSO])
     VALUES
           ('390D97E4-031E-4323-BCBC-711A584C9D16'
           ,'6D672B19-8F61-413E-8277-3351DDDC5C56'
           ,'6B8BCCBB-A9ED-41F3-9B05-A6245AE51B6A'
           ,'All')
GO

INSERT INTO [dbo].[PERFILMODULO]
           ([ID]
           ,[PERFILID]
           ,[MODULOID]
           ,[ACESSO])
     VALUES
           ('389FD420-003E-4578-9DFC-A2AABE0106D8'
           ,'6D672B19-8F61-413E-8277-3351DDDC5C56'
           ,'74EBB025-46DC-4D29-B9B9-42B4FE34557B'
           ,'All')
GO

INSERT INTO [dbo].[PERFILMODULO]
           ([ID]
           ,[PERFILID]
           ,[MODULOID]
           ,[ACESSO])
     VALUES
           ('84E7770B-F48A-4B3B-8DD3-94AE173313D4'
           ,'6D672B19-8F61-413E-8277-3351DDDC5C56'
           ,'4F01042A-4DC0-4794-85F1-E9014341F1BA'
           ,'All')
GO