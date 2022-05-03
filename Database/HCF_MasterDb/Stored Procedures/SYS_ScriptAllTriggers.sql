---- Procedure:
----   [dbo].[SYS_ScriptAllTriggers]
----
---- Parameter: 
----   @ScriptMode bit   
----     possible values:
----     0 - Script ALTER only
----     1 - Script CREATE only
----     2 - Script DROP + CREATE

--Create PROCEDURE [dbo].[SYS_ScriptAllTriggers]
--    @ScriptMode int = 0
--AS 
--BEGIN

--    DECLARE @script TABLE (script varchar(max), id int identity (1,1))

--    DECLARE 
--        @SQL VARCHAR(8000), 
--        @Text            NVARCHAR(4000), 
--        @BlankSpaceAdded INT, 
--        @BasePos         INT, 
--        @CurrentPos      INT, 
--        @TextLength      INT, 
--        @LineId          INT, 
--        @MaxID           INT, 
--        @AddOnLen        INT, 
--        @LFCR            INT, 
--        @DefinedLength   INT, 
--        @SyscomText      NVARCHAR(4000), 
--        @Line            NVARCHAR(1000), 
--        @UserName        SYSNAME, 
--        @ObjID           INT, 
--        @OldTrigID       INT; 

--    SET NOCOUNT ON; 
--    SET @DefinedLength = 1000; 
--    SET @BlankSpaceAdded = 0; 

--    SET @ScriptMode = ISNULL(@ScriptMode, 0);

--    -- This Part Validated the Input parameters   
--    DECLARE @Triggers TABLE (username SYSNAME NOT NULL, trigname SYSNAME NOT NULL, objid INT NOT NULL); 
--    DECLARE @TrigText TABLE (objid INT NOT NULL, lineid INT NOT NULL, linetext NVARCHAR(1000) NULL); 

--    INSERT INTO 
--        @Triggers (username, trigname, objid) 
--    SELECT DISTINCT 
--        OBJECT_SCHEMA_NAME(B.id), B.name, B.id
--    FROM 
--        dbo.sysobjects B, dbo.syscomments C 
--    WHERE 
--        B.type = 'TR' AND B.id = C.id AND C.encrypted = 0; 

--    IF EXISTS(SELECT C.* FROM syscomments C, sysobjects O WHERE O.id = C.id AND O.type = 'TR' AND C.encrypted = 1) 
--    BEGIN 

--        insert into @script select '/*'; 
--        insert into @script select 'The following encrypted triggers were found'; 
--        insert into @script select 'The procedure could not write the script for it'; 

--        insert into 
--            @script 
--        SELECT DISTINCT 
--            '[' + OBJECT_SCHEMA_NAME(B.id) + '].[' + B.name + ']' --, B.id 
--        FROM   
--            dbo.sysobjects B, dbo.syscomments C 
--        WHERE  
--            B.type = 'TR' AND B.id = C.id AND C.encrypted = 1; 

--        insert into @script select '*/'; 
--    END; 

--    DECLARE ms_crs_syscom CURSOR LOCAL forward_only FOR 
--    SELECT 
--        T.objid, C.text
--    FROM   
--        @Triggers T, dbo.syscomments C 
--    WHERE  
--        T.objid = C.id 
--    ORDER  BY T.objid, 
--        C.colid 
--    FOR READ ONLY; 

--    SELECT @LFCR = 2; 
--    SELECT @LineId = 1; 

--    OPEN ms_crs_syscom; 

--    SET @OldTrigID = -1; 

--    FETCH NEXT FROM ms_crs_syscom INTO @ObjID, @SyscomText; 

--    WHILE @@fetch_status = 0 
--    BEGIN 

--        SELECT @BasePos = 1; 
--        SELECT @CurrentPos = 1; 
--        SELECT @TextLength = LEN(@SyscomText); 

--        IF @ObjID <> @OldTrigID 
--        BEGIN 
--            SET @LineID = 1; 
--            SET @OldTrigID = @ObjID; 
--        END; 

--        WHILE @CurrentPos != 0 
--        BEGIN 
--            --Looking for end of line followed by carriage return         
--            SELECT @CurrentPos = CHARINDEX(CHAR(13) + CHAR(10), @SyscomText, @BasePos); 

--            --If carriage return found         
--            IF @CurrentPos != 0 
--            BEGIN 

--                WHILE ( ISNULL(LEN(@Line), 0) + @BlankSpaceAdded + @CurrentPos - @BasePos + @LFCR ) > @DefinedLength 
--                BEGIN 
--                    SELECT @AddOnLen = @DefinedLength - (ISNULL(LEN(@Line), 0) + @BlankSpaceAdded ); 

--                    INSERT 
--                        @TrigText 
--                    VALUES 
--                        ( @ObjID, @LineId, ISNULL(@Line, N'') + ISNULL(SUBSTRING(@SyscomText, @BasePos, @AddOnLen), N'')); 

--                    SELECT 
--                        @Line = NULL, 
--                        @LineId = @LineId + 1, 
--                        @BasePos = @BasePos + @AddOnLen, 
--                        @BlankSpaceAdded = 0; 
--                END; 

--                SELECT @Line = ISNULL(@Line, N'') + ISNULL(SUBSTRING(@SyscomText, @BasePos, @CurrentPos - @BasePos + @LFCR), N''); 

--                SELECT @BasePos = @CurrentPos + 2; 

--                INSERT 
--                    @TrigText 
--                VALUES
--                    ( @ObjID, @LineId, @Line ); 

--                SELECT @LineId = @LineId + 1; 

--                SELECT @Line = NULL; 
--            END; 
--            ELSE 
--            --else carriage return not found         
--            BEGIN 
--                IF @BasePos <= @TextLength 
--                BEGIN 
--                    /*If new value for @Lines length will be > then the         
--                    **defined length         
--                    */ 
--                    WHILE ( ISNULL(LEN(@Line), 0) + @BlankSpaceAdded + @TextLength - @BasePos + 1 ) > @DefinedLength 
--                    BEGIN 
--                        SELECT @AddOnLen = @DefinedLength - ( ISNULL(LEN(@Line), 0 ) + @BlankSpaceAdded ); 

--                        INSERT 
--                            @TrigText 
--                        VALUES 
--                            ( @ObjID, @LineId, ISNULL(@Line, N'') + ISNULL(SUBSTRING(@SyscomText, @BasePos, @AddOnLen), N'')); 

--                        SELECT 
--                            @Line = NULL, 
--                            @LineId = @LineId + 1, 
--                            @BasePos = @BasePos + @AddOnLen, 
--                            @BlankSpaceAdded = 0; 
--                    END; 

--                    SELECT @Line = ISNULL(@Line, N'') + ISNULL(SUBSTRING(@SyscomText, @BasePos, @TextLength - @BasePos+1 ), N''); 

--                    IF LEN(@Line) < @DefinedLength AND CHARINDEX(' ', @SyscomText, @TextLength + 1) > 0 
--                    BEGIN 
--                        SELECT 
--                            @Line = @Line + ' ', 
--                            @BlankSpaceAdded = 1; 
--                    END; 
--                END; 
--            END; 
--        END; 

--        FETCH NEXT FROM ms_crs_syscom INTO @ObjID, @SyscomText; 
--    END; 

--    IF @Line IS NOT NULL 
--        INSERT 
--            @TrigText 
--        VALUES
--            ( @ObjID, @LineId, @Line ); 

--    CLOSE ms_crs_syscom; 

--    insert into @script select '-- You should run this result under dbo if your triggers belong to multiple users'; 
--    insert into @script select ''; 

--    IF @ScriptMode = 2 
--    BEGIN 

--        insert into @script select '-- Dropping the Triggers'; 
--        insert into @script select ''; 

--        insert into @script 
--        SELECT 
--            'IF EXISTS(SELECT * FROM sysobjects WHERE id = OBJECT_ID(''[' + username + '].[' + trigname + ']'')'
--            + ' AND ObjectProperty(OBJECT_ID(''[' + username + '].[' + trigname + ']''), ''ISTRIGGER'') = 1)'
--            + ' DROP TRIGGER [' + username + '].[' + trigname +']' + CHAR(13) + CHAR(10) 
--            + 'GO' + CHAR(13) + CHAR(10)
--        FROM   
--            @Triggers; 
--    END; 

--    IF @ScriptMode = 0
--    BEGIN   
--        update 
--            @TrigText 
--        set 
--            linetext = replace(linetext, 'CREATE TRIGGER', 'ALTER TRIGGER') 
--        WHERE 
--            upper(left(replace(ltrim(linetext), char(9), ''), 14)) = 'CREATE TRIGGER' 
--    END

--    insert into @script select '----------------------------------------------'; 
--    insert into @script select '-- Creation of Triggers'; 
--    insert into @script select ''; 
--    insert into @script select ''; 

--    DECLARE ms_users CURSOR LOCAL forward_only FOR 
--    SELECT 
--        T.username, 
--        T.objid, 
--        MAX(D.lineid) 
--    FROM   
--        @Triggers T, 
--        @TrigText D 
--    WHERE  
--        T.objid = D.objid 
--    GROUP BY    
--        T.username, 
--        T.objid 
--    FOR READ ONLY; 

--    OPEN ms_users; 

--    FETCH NEXT FROM ms_users INTO @UserName, @ObjID, @MaxID; 

--    WHILE @@fetch_status = 0 
--    BEGIN 

--        insert into @script select 'setuser N''' + @UserName + '''' + CHAR(13) + CHAR(10); 

--        insert into @script 
--        SELECT 
--            '-- Text of the Trigger' = 
--            CASE lineid 
--                WHEN 1 THEN 'GO' + CHAR(13) + CHAR(10) + linetext 
--                WHEN @MaxID THEN linetext + 'GO' 
--                ELSE linetext 
--            END 
--        FROM   
--            @TrigText 
--        WHERE  
--            objid = @ObjID 
--        ORDER  
--            BY lineid; 

--        insert into @script select 'setuser'; 

--        FETCH NEXT FROM ms_users INTO @UserName, @ObjID, @MaxID; 
--    END; 

--    CLOSE ms_users; 

--    insert into @script select 'GO'; 
--    insert into @script select '------End ------'; 

--    DEALLOCATE ms_crs_syscom; 
--    DEALLOCATE ms_users; 

--    select script from @script order by id

--END
