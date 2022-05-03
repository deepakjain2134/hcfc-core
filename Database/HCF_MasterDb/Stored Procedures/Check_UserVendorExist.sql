Create Procedure [dbo].[Check_UserVendorExist] --'admin@hcf.com',0
(
@vendorId int 
)
As
Begin
SELECT CASE WHEN EXISTS (
    select 1 from dbo.UserProfile where VendorId = @vendorId
)
THEN CAST(1 AS BIT)
ELSE CAST(0 AS BIT) END 
end