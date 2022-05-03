CREATE Proc [dbo].[Get_Organizations] --'0D320358-9DC0-4302-AC04-7E39FB65B8BD'
@Orgkey uniqueidentifier =null,
@ClientNo int = null

As
Begin
Select Isnull(IM.EmailId,'') as 'CRxInboxMailId', Org.* from Organization Org 
left join InboxEmails IM on IM.CLientNo=Org.ClientNo
Where Org.Orgkey=ISNULL(@Orgkey,Org.Orgkey) 
or ParentOrgKey=ISNULL(@Orgkey,ParentOrgKey) 
or Org.ClientNo = ISNULL(@ClientNo,Org.ClientNo) 

end