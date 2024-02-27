using API.Models.Roles;
using API.Models.Tags;
using MyBlog.BLL.Models;

namespace API.Models.Articles
{
    public class GetArticlesResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Text { get; set; }
        public User Author { get; set; }
        public DateTime DateTimeArticle { get; set; }
        public List<TagViews> Tags { get; set; } = new List<TagViews>();
    }
}
