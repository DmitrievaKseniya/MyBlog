namespace API.Models.Articles
{
    public class EditArticleRequest
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string? Text { get; set; }
    }
}
