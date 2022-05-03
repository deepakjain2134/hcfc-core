CREATE VIEW [dbo].[view_cmsEPs]
AS
SELECT        CopDetails.CopDetailsId, CopDetails.CopStdescId,CopDetails.RequirementName, CopDetails.CopText, CopDetails.EPTextID, CopStdesc.CopName, CopStdesc.CopTitle, 
                         CopStdesc.TagCode
FROM            CopDetails INNER JOIN
                         CopStdesc ON CopDetails.CopStdescId = CopStdesc.CopStdescId

GO