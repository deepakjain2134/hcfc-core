CREATE proc [dbo].[Get_Manufacture]
As
BEgin

Select Manufactures.*,UP.FirstName,UP.LastName,UP.Email from Manufactures 
inner join UserProfile UP on UP.UserId=Manufactures.CreatedBy 
order by ManufactureName

END



