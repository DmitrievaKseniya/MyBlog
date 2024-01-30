using MyBlog.BLL.Models;
using MyBlog.BLL.ViewModels;

namespace MyBlog.WebService.Extentions
{
    public static class CommentFromModel
    {
        public static Comment Convert(this Comment comment, CommentEditViewModel commentEditVM)
        {
            comment.Text = commentEditVM.Text;

            return comment;
        }
    }
}
