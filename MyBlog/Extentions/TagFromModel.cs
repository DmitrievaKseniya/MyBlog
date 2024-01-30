using MyBlog.BLL.Models;
using MyBlog.BLL.ViewModels;

namespace MyBlog.WebService.Extentions
{
    public static class TagFromModel
    {
        public static Tag Convert(this Tag tag, TagEditViewModel tagEditVM)
        {
            tag.Name = tagEditVM.Name;

            return tag;
        }
    }
}
