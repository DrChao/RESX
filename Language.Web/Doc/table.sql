if exists(Select * From sysobjects where xtype='U' and name='Resx_DetailInfo') 
Begin	
	Drop Table dbo.Resx_DetailInfo
end
GO
Create Table  Resx_DetailInfo(
	FKey			nvarchar(50)	NULL,
	Resx_Type		nvarchar(50)	NULL,
	Resx_Text		nvarchar(3000)  NULL,
	Sys_Name		nvarchar(50)	NULL,
	CUser			nvarchar(50)	NULL,
	CTime			smalldatetime	NULL
)  

if exists(Select * From sysobjects where xtype='U' and name='Resx_Info') 
Begin	
	Drop Table dbo.Resx_Info
end
GO
CREATE TABLE dbo.Resx_Info(
	FKey			nvarchar(50)		NOT NULL primary key,
	FValue			nvarchar(3000)		NULL,
	Is_JScript		bit					NULL	DEFAULT ((0)),
	Is_Translate	bit					NULL	DEFAULT ((0)),
	Sys_Name		nvarchar(50)		NULL,
	CUser			nvarchar(50)		NULL,
	CTime			smalldatetime		NULL
)
GO

------------------------------------------------------Sql日志记录------------------------------------------------------
If Exists (Select * from dbo.sysobjects where id = object_id(N'cpc_Sql_CommandLog') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
		Drop   proc   cpc_Sql_CommandLog
End
GO
Create procedure [dbo].[cpc_Sql_CommandLog]
(
	@CommandName		nvarchar(2000),
	@CommandVariable	nvarchar(1000),
	@CommandType		nvarchar(50),
	@DataType			nvarchar(50),
	@UserName			nvarchar(50)
)
AS
	Begin
		If Not Exists (Select  table_name from information_schema.tables Where table_name ='cpc_Sql_Log')
		Begin
			Create Table dbo.cpc_Sql_Log
			(
				ID								nvarchar(50)			Not Null Default NewID(),
				CommandName		nvarchar(2000),
				CommandVariable	nvarchar(1000),
				CommandType			nvarchar(50),
				DataBaseType			nvarchar(50),
				UserName				nvarchar(50),
				ExecuteTime				smalldatetime
			)
		End

		Insert into  cpc_Sql_Log(CommandName,CommandVariable,CommandType,DataBaseType,UserName,ExecuteTime)
		values (@CommandName ,@CommandVariable ,@CommandType,@DataType,@UserName,GetDate() )
	End
  