using API.Models.Articles;
using API.Models.Users;
using MyBlog.BLL.Models;

namespace API.Models.Comments
{
    public class GetCommentsResponse
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string AuthorId { get; set; }
        public int ArticleId { get; set; }
    }
}
