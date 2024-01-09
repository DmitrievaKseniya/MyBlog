using BusinessLogicLayer.Models;
using BusinessLogicLayer.ViewModels;

namespace MyBlog.Extentions
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
