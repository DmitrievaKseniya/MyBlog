using MyBlog.BLL.Models;
using MyBlog.BLL.ViewModels;

namespace MyBlog.WebService.Extentions
{
    public static class RoleFromModel
    {
        public static Role Convert(this Role role, RoleEditViewModel roleEditVM)
        {
            role.Name = roleEditVM.Name;
            role.Description = roleEditVM.Description;
            
            return role;
        }
    }
}
