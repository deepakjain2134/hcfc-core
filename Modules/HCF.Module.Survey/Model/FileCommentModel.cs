using System;

namespace HCF.Module.Survey.Model
{
    public class FileCommentModel
    {
        public FileCommentModel()
        {
            CommentDateTime = DateTime.UtcNow;
        }

        public FileCommentModel(int fileId, int commentBy,string comment )
        {
            FileId = fileId;
            Comment = comment;
            CommentBy = commentBy;
            IsDeleted = false;
            CommentDateTime = DateTime.UtcNow;

        }
        public string Comment { get; set; }

        public DateTime CommentDateTime { get; set; }

        public int CommentBy { get; set; }

        public int FileId { get; set; }

        public bool IsDeleted { get; set; }

    }
}
