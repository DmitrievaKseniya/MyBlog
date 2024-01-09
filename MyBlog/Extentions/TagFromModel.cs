using BusinessLogicLayer.Models;
using BusinessLogicLayer.ViewModels;

namespace MyBlog.Extentions
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
