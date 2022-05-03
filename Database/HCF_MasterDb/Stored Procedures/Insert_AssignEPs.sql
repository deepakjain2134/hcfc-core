CREATE procedure [dbo].[Insert_AssignEPs]
@epdetailId int= null,
@epgroupId int =null,
@status bit,
@createdby int =null
--@newId int output
As
Begin 
IF EXISTS (SELECT 1 FROM  [EPGroupsDetail] WHERE EPDetailId=@epdetailId and EPGroupId=@epgroupId )
	BEGIN
		UPDATE EPGroupsDetail
		SET [IsActive] = @status
		WHERE EPDetailId=@epdetailId and EPGroupId=@epgroupId
   	    return @epgroupId
	END
	ELSE
	BEGIN
		 INSERT INTO [dbo].[EPGroupsDetail]
				   ([EPGroupId]
				   ,[EPDetailId]
				   ,[IsActive])
	     VALUES
				   (@epgroupId,
				  @epdetailId,
				   @status)
			 --   select @newId = Scope_Identity()
				--return @newId
	 END
END
