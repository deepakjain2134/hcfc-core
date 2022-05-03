-- =============================================  
-- Author:  PK  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[Get_All_Tips]   
 -- Add the parameters for the stored procedure here  
 @clientNo int = null  
AS  
BEGIN  
   
 SELECT * from Tips left join Menus on Menus.Alias=Tips.RouteUrl --Where IsActive=1 --and IsApproved=1-- where (ClientNo=isnull(@clientNo,ClientNo) OR ClientNo is null) and (IsCurrent=1 or IsApproved=2);  
END  