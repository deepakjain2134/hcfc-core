create Procedure [dbo].[Delete_News] 
(
@Id int
)
As
Begin
 Update News set IsDeleted=1 where Id= @Id 
end
