namespace API.Models.Comments
{
    public class EditCommentRequest
    {
        public string Text { get; set; }
        public string AuthorId { get; set; }
        public int ArticleId { get; set; }
    }
}
