CREATE PROCEDURE [dbo].[SetArchiveRound]


    @DB VARCHAR(50)

AS

BEGIN



SET NOCOUNT ON;



DECLARE @STMT VARCHAR( 300 );

DECLARE @SP VARCHAR( 500 );





SET @SP = 'dbo.SetArchiveRound';





SET @STMT = 'EXEC(''' +  @SP + ''')';   





EXEC('USE '+ @db + ';' + @STMT)



END
