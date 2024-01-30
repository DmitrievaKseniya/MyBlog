using MyBlog.BLL.Models;
using MyBlog.BLL.ViewModels;
using MyBlog.DAL.Repository;
using MyBlog.DAL.UoW;

namespace MyBlog.WebService.Extentions
{
    public static class ArticleFromModel
    {
        public static Article Convert(this Article article, ArticleEditViewModel articleEditVM, List<Tag> selectedTags)
        {
            article.Title = articleEditVM.Title;
            article.Text = articleEditVM.Text;
            article.Tags = selectedTags;

            return article;
        }
    }
}
