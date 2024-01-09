using BusinessLogicLayer.Models;
using BusinessLogicLayer.ViewModels;

namespace MyBlog.Extentions
{
    public static class ArticleFromModel
    {
        public static Article Convert(this Article article, ArticleEditViewModel articleEditVM)
        {
            article.Title = articleEditVM.Title;
            article.Text = articleEditVM.Text;

            return article;
        }
    }
}
