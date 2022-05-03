CREATE proc [dbo].[Insert_EPVersions]
@EPdetailId int,
@tableName nvarchar(500),
@ModifiedTime datetime,
@CreatedBy int
As
Begin
DECLARE @EPVersionId uniqueidentifier = newId();
DECLARE @EPDescriptionId nvarchar(50);
DECLARE @ScoreId int;
DECLARE @FrequencyId int;
DECLARE @AssetId int;
DECLARE @DocTypeId int;
DECLARE @BinderId int;
--DECLARE @HospitalTypeId int;
--select @HospitalTypeId = HospitalTypeId from Organization
select @EPDescriptionId = EPDescriptionId from [dbo].EPDescriptions where EPDetailId = @EPdetailId 
--and HospitalTypeId = @HospitalTypeId
select @ScoreId = ScoreId from .[dbo].EPDetails where EPDetailId = @EPdetailId and IsActive =1
select @AssetId = AssetId from .[dbo].EpAssets where EPDetailId =@EPdetailId and IsActive =1
select @FrequencyId = TjcFrequencyId from .[dbo].EPFrequency where EpDetailId = @EPdetailId and IsActive =1
select @DocTypeId = DocTypeId from .[dbo].EPDocuments where EPDetailId = @EPdetailId AND IsActive = 1
select @BinderId = BinderId from .[dbo].EpBinder where EPDetailId = @EPdetailId ANd IsActive = 1

if not exists(select 1 from [dbo].EPVersions where IsCurrent =1)
   update .[dbo].EPVersions set IsCurrent = 0 where EPDetailId = @EPdetailId
	BEGIN
	INSERT INTO [dbo].[EPVersions]([EPVersionId],[EPDetailId],[EPDescriptionId],[ScoreId],[FrequencyId],[AssetId]
			   ,[DocTypeId],[BinderId],[ModifiedType],[CreatedBy],[CreatedDate])
		 VALUES
			   (@EPVersionId,@EPdetailId,@EPDescriptionId,@ScoreId,@FrequencyId,@AssetId,@DocTypeId,@BinderId,
				@tableName,@CreatedBy,@ModifiedTime)
	END
END



