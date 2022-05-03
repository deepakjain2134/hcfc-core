-- =============================================
-- Author:  Anoop
-- ALTER date: 2016-11-04 05:40:16.700
-- Description: Pass comma separated (delimited) values as Parameter to Stored Procedure in SQL Server
-- =============================================
Create FUNCTION [dbo].[SplitString]
(    
      @Input NVARCHAR(MAX),
      @Character CHAR(1)
)
RETURNS @Output TABLE (
      Item NVARCHAR(MAX),
   Row# int
)
AS
BEGIN
      DECLARE @StartIndex INT, @EndIndex INT
      DECLARE @Sequence INT
      SET @StartIndex = 1

	  -- remove last comma
	  Select @Input= case when right(rtrim(@Input),1) = ',' then substring(rtrim(@Input),1,len(rtrim(@Input))-1) else @Input END 

   Set @Sequence=0;
      IF SUBSTRING(@Input, LEN(@Input) - 1, LEN(@Input)) <> @Character
      BEGIN
            SET @Input = @Input + @Character
      END
      WHILE CHARINDEX(@Character, @Input) > 0
      BEGIN
            SET @EndIndex = CHARINDEX(@Character, @Input)
            INSERT INTO @Output(Item,Row#)
            SELECT SUBSTRING(LTrim(@Input), @StartIndex, @EndIndex - 1),@Sequence
            SET @Sequence = @Sequence + 1
            SET @Input = SUBSTRING(@Input, @EndIndex + 1, LEN(@Input))
      END
      RETURN
END